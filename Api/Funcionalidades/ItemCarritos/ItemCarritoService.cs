using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Persistencia;
using Api.Funcionalidades.Auth;
using Microsoft.AspNetCore.Http;
using Biblioteca.Dominio;
using Microsoft.EntityFrameworkCore;

namespace Api.Funcionalidades.ItemCarritos
{
    public class ItemCarritoService : IItemCarritoServices
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ItemCarritoService(AppDbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<ItemCarrito> GetItemCarrito()
        {
            _authService.AuthenticationAdmin();
            return _context.ItemCarrito.ToList();
        }

        public void AddItemCarrito(ItemCarrito itemCarrito)
        {
            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != itemCarrito.Carrito.UsuarioId)
            {
                throw new UnauthorizedAccessException("No puedes agregar un item a un carrito que no es tuyo");
            }

            var producto = _context.Producto.Include(p => p.HistorialPrecios.OrderByDescending(h => h.FechaCambio).FirstOrDefault()).FirstOrDefault(p => p.Id == itemCarrito.Producto.Id);
            if(producto == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }
            if(producto.Stock < itemCarrito.Cantidad)
            {
                throw new ArgumentException("No hay suficiente stock para agregar al carrito");
            }

            itemCarrito.Subtotal = producto.HistorialPrecios.OrderByDescending(h => h.FechaCambio).FirstOrDefault().Precio * itemCarrito.Cantidad;
            _context.ItemCarrito.Add(itemCarrito);
            _context.SaveChanges();
        }

        public void UpdateItemCarrito(ItemCarrito itemCarrito)
        {
            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != itemCarrito.Carrito.UsuarioId || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["rol"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes actualizar un item de un carrito que no es tuyo");
            }
            _context.ItemCarrito.Update(itemCarrito);
            _context.SaveChanges();
        }

        public void DeleteItemCarrito(Guid id)
        {
            var carrito = _context.Carrito.Include(c => c.Items).ThenInclude(i => i.Producto).FirstOrDefault(c => c.Items.Any(i => i.Id == id));
            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != carrito.UsuarioId || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["rol"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes eliminar un item de un carrito que no es tuyo");
            }
            if(carrito == null)
            {
                throw new ArgumentException("Carrito no encontrado");
            }
            if(carrito.Entregado)
            {
                throw new ArgumentException("Carrito ya entregado");
            }
            var item = _context.ItemCarrito.Include(i => i.Producto).FirstOrDefault(i => i.Id == id);
            if(item == null)
            {
                throw new ArgumentException("Item no encontrado");
            }
            if(item.Entregado)
            {
                throw new ArgumentException("Item ya entregado");
            }
            _context.ItemCarrito.Remove(item);
            _context.SaveChanges();
        }
        
        public List<ItemCarrito> GetProductosVendedorId(Guid id)
        {
            _authService.AuthenticationVendedoryAdministrador();

            return _context.ItemCarrito.Include(i => i.Producto).Where(i => i.Producto.VendedorId == id && i.Entregado == false).ToList();

            throw new UnauthorizedAccessException("No tienes permisos para ver los productos de un vendedor");
        }

        public void MarcarComoEntregadoItem(Guid id)
        {
            var item = _context.ItemCarrito.Include(i => i.Producto).FirstOrDefault(i => i.Id == id);
            if(item == null)
            {
                throw new ArgumentException("Item no encontrado");
            }
            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != item.Producto.VendedorId || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes marcar un item como entregado si no es tuyo");
            }
            item.Entregado = true;
            _context.SaveChanges();
        }

        public void RechazarItem(Guid id)
        {
            var item = _context.ItemCarrito.Include(i => i.Producto).FirstOrDefault(i => i.Id == id);
            if(item == null)
            {
                throw new ArgumentException("Item no encontrado");
            }
            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != item.Producto.VendedorId || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes rechazar un item si no es tuyo");
            }

            if(item.Entregado)
            {
                throw new ArgumentException("Item ya entregado");
            }

            _context.ItemCarrito.Remove(item);
            _context.Producto.Find(item.Producto.Id).Stock += item.Cantidad;
            _context.SaveChanges();
        }
    }

    public interface IItemCarritoServices
    {
        List<ItemCarrito> GetItemCarrito();
        void AddItemCarrito(ItemCarrito itemCarrito);
        void UpdateItemCarrito(ItemCarrito itemCarrito);
        void DeleteItemCarrito(Guid id);
        List<ItemCarrito> GetProductosVendedorId(Guid id);
        void MarcarComoEntregadoItem(Guid id);
        void RechazarItem(Guid id);
    }
}