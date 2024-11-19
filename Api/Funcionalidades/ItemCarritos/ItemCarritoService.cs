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
    /// <summary>
    /// Servicio que maneja la lógica de negocio para los items del carrito de compras
    /// </summary>
    public class ItemCarritoService : IItemCarritoServices
    {
        // Dependencias inyectadas
        private readonly AppDbContext _context;                     // Contexto de base de datos
        private readonly IAuthService _authService;                // Servicio de autenticación
        private readonly IHttpContextAccessor _httpContextAccessor; // Acceso al contexto HTTP

        /// <summary>
        /// Constructor que inicializa las dependencias necesarias
        /// </summary>
        public ItemCarritoService(AppDbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Obtiene los items de un carrito específico
        /// </summary>
        /// <param name="carritoId">ID del carrito a consultar</param>
        /// <returns>Lista de items del carrito</returns>
        public List<ItemCarritoSelectDto> GetItemCarritoPorCarritoId(Guid carritoId)
        {
            var id = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
            var rol = _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
            var carrito = _context.Carrito.AsNoTracking().Include(c => c.Items).FirstOrDefault(c => c.Id == carritoId);
            if(carrito == null)
            {
                throw new ArgumentException("Carrito no encontrado");
            }
            if(rol == "Administrador")
            {
                return carrito.Items.Select(i => new ItemCarritoSelectDto
                {
                    Id = i.Id,
                    ProductoId = null,
                    Cantidad = i.Cantidad,
                    Subtotal = i.Subtotal,
                    Entregado = i.Entregado
                }).ToList();
            }
            return carrito.Items.Where(i => i.Carrito.UsuarioId == id).Select(i => new ItemCarritoSelectDto
            {
                Id = i.Id,
                ProductoId = null,
                Cantidad = i.Cantidad,
                Subtotal = i.Subtotal,
                Entregado = i.Entregado
            }).ToList();
        }

        /// <summary>
        /// Agrega un nuevo item al carrito
        /// </summary>
        /// <param name="itemCarritoDto">Datos del item a agregar</param>
        /// <exception cref="ArgumentException">Si el producto no existe o no hay stock suficiente</exception>
        /// <exception cref="UnauthorizedAccessException">Si el usuario no tiene permisos</exception>
        public void AddItemCarrito(ItemCarritoDto itemCarritoDto)
        {

            var itemCarrito = new ItemCarrito();
            if(itemCarritoDto.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a 0");
            }
            
            var carrito = new Carrito();
            if(itemCarritoDto.CarritoId != Guid.Empty)
            {
                carrito = _context.Carrito.FirstOrDefault(c => c.Id == itemCarritoDto.CarritoId);
                if(carrito == null)
                {
                    throw new ArgumentException("Carrito no encontrado");
                }
            }
            else
            {
                carrito = new Carrito { UsuarioId = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) };
                _context.Carrito.Add(carrito);
            }
            
            itemCarrito.Carrito = carrito;

            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != itemCarrito.Carrito.UsuarioId)
            {
                throw new UnauthorizedAccessException("No puedes agregar un item a un carrito que no es tuyo");
            }

            var producto = _context.Producto.Include(p => p.HistorialPrecios).FirstOrDefault(p => p.Id == itemCarritoDto.ProductoId);
            if(producto == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }
            if(producto.Stock < itemCarritoDto.Cantidad)
            {
                throw new ArgumentException("No hay suficiente stock para agregar al carrito");
            }
            itemCarrito.Producto = producto;

            var historialPrecio = producto.HistorialPrecios.OrderByDescending(h => h.FechaCambio).FirstOrDefault();
            if(historialPrecio == null)
            {
                throw new ArgumentException("No hay historial de precios para el producto");
            }
            if(producto.Stock < itemCarrito.Cantidad)
            {
                throw new ArgumentException("No hay suficiente stock para agregar al carrito");
            }
            itemCarrito.Subtotal = historialPrecio.Precio * itemCarritoDto.Cantidad;
            itemCarrito.Cantidad = itemCarritoDto.Cantidad;
            _context.ItemCarrito.Add(itemCarrito);
            _context.SaveChanges();
        }

        /// <summary>
        /// Actualiza la cantidad de un item en el carrito
        /// </summary>
        /// <param name="itemCarritoDto">Nuevos datos del item</param>
        /// <param name="id">ID del item a actualizar</param>
        public void UpdateItemCarrito(ItemCarritoUpdateDto itemCarritoDto, Guid id)
        {

            var itemCarrito = _context.ItemCarrito.Include(i => i.Producto).Include(i => i.Carrito).FirstOrDefault(i => i.Id == id);
            if(itemCarrito == null)
            {
                throw new ArgumentException("Item no encontrado");
            }

            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != itemCarrito.Carrito.UsuarioId)
            {
                if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
                {
                    throw new UnauthorizedAccessException("No puedes actualizar un item de un carrito que no es tuyo");
                }
            }

            if(itemCarritoDto.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a 0");
            }
            itemCarrito.Cantidad = itemCarritoDto.Cantidad;

            var producto = _context.Producto.Include(p => p.HistorialPrecios).FirstOrDefault(p => p.Id == itemCarrito.Producto.Id);
            if(producto == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }

            var historialPrecio = producto.HistorialPrecios.OrderByDescending(h => h.FechaCambio).FirstOrDefault();
            if(historialPrecio == null)
            {
                throw new ArgumentException("No hay historial de precios para el producto");
            }
            if(producto.Stock < itemCarrito.Cantidad)
            {
                throw new ArgumentException("No hay suficiente stock para agregar al carrito");
            }

            itemCarrito.Subtotal = historialPrecio.Precio * itemCarritoDto.Cantidad;
            itemCarrito.Cantidad = itemCarritoDto.Cantidad;
            _context.ItemCarrito.Update(itemCarrito);
            _context.SaveChanges();
        }

        /// <summary>
        /// Elimina un item del carrito
        /// </summary>
        /// <param name="id">ID del item a eliminar</param>
        public void DeleteItemCarrito(Guid id)
        {
            var carrito = _context.Carrito.Include(c => c.Items).ThenInclude(i => i.Producto).FirstOrDefault(c => c.Items.Any(i => i.Id == id));
            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != carrito.UsuarioId || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
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
        
        /// <summary>
        /// Obtiene los productos de un vendedor específico
        /// </summary>
        /// <param name="id">ID del vendedor</param>
        /// <returns>Lista de items del carrito asociados al vendedor</returns>
        public List<ItemCarrito> GetProductosVendedorId(Guid id)
        {
            _authService.AuthenticationVendedoryAdministrador();

            return _context.ItemCarrito.Include(i => i.Producto).Where(i => i.Producto.VendedorId == id && i.Entregado == false).ToList();

            throw new UnauthorizedAccessException("No tienes permisos para ver los productos de un vendedor");
        }

        /// <summary>
        /// Marca un item como entregado
        /// </summary>
        /// <param name="id">ID del item</param>
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

        /// <summary>
        /// Rechaza un item del carrito y restaura el stock
        /// </summary>
        /// <param name="id">ID del item</param>
        public void RechazarItem(Guid id)
        {
            var item = _context.ItemCarrito.Include(i => i.Producto).FirstOrDefault(i => i.Id == id);
            if(item == null)
            {
                throw new ArgumentException("Item no encontrado");
            }
            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != item.Producto.VendedorId)
            {
                if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
                {
                    throw new UnauthorizedAccessException("No puedes rechazar un item si no es tuyo");
                }
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

    /// <summary>
    /// Interfaz que define los métodos del servicio de items del carrito
    /// </summary>
    public interface IItemCarritoServices
    {
        List<ItemCarritoSelectDto> GetItemCarritoPorCarritoId(Guid carritoId);
        void AddItemCarrito(ItemCarritoDto itemCarritoDto);
        void UpdateItemCarrito(ItemCarritoUpdateDto itemCarritoDto, Guid id);
        void DeleteItemCarrito(Guid id);
        List<ItemCarrito> GetProductosVendedorId(Guid id);
        void MarcarComoEntregadoItem(Guid id);
        void RechazarItem(Guid id);
    }
}