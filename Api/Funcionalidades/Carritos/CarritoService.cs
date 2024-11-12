using Api.Persistencia;
using Biblioteca.Dominio;
using Microsoft.EntityFrameworkCore;
using Api.Funcionalidades.Auth;
namespace Api.Funcionalidades.Carritos;

public class CarritoService : ICarritoService
{
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CarritoService(AppDbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public void DeleteCarrito(Guid id)
    {
        var carrito = _context.Carrito.Find(id);
        if (carrito != null)
        {
            if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != carrito.UsuarioId)
            {
                if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
                {
                    throw new UnauthorizedAccessException("No puedes eliminar un carrito que no sea tuyo");
                }
            }
            _context.Carrito.Remove(carrito);
            _context.SaveChanges();
        }
    }

    public List<CarritoDto> GetCarritoPorUsuario()
    {
        var rol = _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        if(rol == "Administrador")
        {
            return _context.Carrito.Include(c => c.Items).Select(c => new CarritoDto
            {
                Id = c.Id,
                UsuarioId = c.UsuarioId,
                Entregado = c.Entregado,
                Pagado = c.Pagado,
                Eliminado = c.Eliminado,
                Items = c.Items.Select(i => i.Id).ToList()
            }).ToList();
        }
        var id = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        return _context.Carrito.Include(c => c.Items).Where(c => c.UsuarioId == id && !c.Eliminado).Select(c => new CarritoDto
        {
            Id = c.Id,
            UsuarioId = c.UsuarioId,
            Entregado = c.Entregado,
            Pagado = c.Pagado,
            Eliminado = c.Eliminado,
            Items = c.Items.Select(i => i.Id).ToList()
        }).ToList();
    }


    public List<CarritoDto> BuscarCarritoPorProducto(Guid productoId)
    {
        var rol = _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        if(rol == "Administrador")
        {
            return _context.Carrito
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .Where(c => c.Items.Any(i => i.Producto.Id == productoId))
                .Select(c => new CarritoDto
                {
                Id = c.Id,
                UsuarioId = c.UsuarioId,
                Entregado = c.Entregado,
                Pagado = c.Pagado,
                Eliminado = c.Eliminado,
                Items = c.Items.Select(i => i.Id).ToList()
            }).ToList();
        }

        var id = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        return _context.Carrito
            .Include(c => c.Items)
            .ThenInclude(i => i.Producto)
            .Where(c => c.Items.Any(i => i.Producto.Id == productoId && c.UsuarioId == id) && !c.Eliminado)
            .Select(c => new CarritoDto
            {
                Id = c.Id,
                UsuarioId = c.UsuarioId,
                Entregado = c.Entregado,
                Pagado = c.Pagado,
                Eliminado = c.Eliminado,
                Items = c.Items.Select(i => i.Id).ToList()
            }).ToList();
    }

    public void MarcarComoEntregado(Guid id)//Cambia el estado del carrito a entregado(true).
    {

        var carrito = _context.Carrito.Find(id);
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != carrito.UsuarioId )
        {
            if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes marcar un carrito como entregado si no es tuyo");
            }
        }

        if (carrito != null)
        {
            if(carrito.Eliminado)
            {
                throw new ArgumentException("Carrito eliminado");
            }
            carrito.Entregado = true;
            _context.SaveChanges();
        }
    }

    public decimal CalcularTotal(Guid id)
    {
        var carrito = _context.Carrito
            .Include(c => c.Items)
            .FirstOrDefault(c => c.Id == id);
            
        if (carrito == null) return 0;

        decimal total = 0;
        foreach (var item in carrito.Items)
        {
            total += item.Subtotal;
        }

        return total;
    }

    public void MarcarComoEliminado(Guid id) //Cambia el estado del carrito a eliminado.No elimina el carrito de la base de datos.
    {
        var carrito = _context.Carrito.Find(id);
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != carrito.UsuarioId )
        {
            if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes marcar un carrito como eliminado si no es tuyo");
            }
        }

        if (carrito != null)
        {
            if(carrito.Pagado)
            {
                throw new ArgumentException("Carrito pagado");
            }
            if(carrito.Eliminado)
            {
                throw new ArgumentException("Carrito eliminado");
            }
            if(carrito.Entregado)
            {
                throw new ArgumentException("Carrito entregado");
            }
            carrito.Eliminado = true;
            _context.SaveChanges();
        }
    }

    public void PagarCarrito(Guid id)
    {
        var carrito = _context.Carrito.Include(c => c.Items).ThenInclude(i => i.Producto).FirstOrDefault(c => c.Id == id);
        if(carrito == null)
        {
            throw new ArgumentException("Carrito no encontrado");
        }

        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != carrito.UsuarioId || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
        {
            throw new UnauthorizedAccessException("No puedes pagar un carrito que no sea tuyo");
        }

        if(carrito.Eliminado)
        {
            throw new ArgumentException("Carrito eliminado");
        }

        if(carrito.Pagado)
        {
            throw new ArgumentException("Carrito ya pagado");
        }
        // aca tendria que agregar el proceso de pago por parte de terceros.
        carrito.Pagado = true;

        foreach (var item in carrito.Items)
        {
            var producto = _context.Producto.Find(item.Producto.Id);
            if(producto == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }
            if(producto.Stock < item.Cantidad)
            {
                throw new ArgumentException("Stock insuficiente");
            }
            producto.Stock -= item.Cantidad;
        }
        carrito.Total = CalcularTotal(id);
        _context.SaveChanges();
    }

}

public interface ICarritoService
{
    void DeleteCarrito(Guid id);
    List<CarritoDto> GetCarritoPorUsuario();
    List<CarritoDto> BuscarCarritoPorProducto(Guid id);
    void MarcarComoEntregado(Guid id);
    decimal CalcularTotal(Guid id);
    void MarcarComoEliminado(Guid id);
    void PagarCarrito(Guid id);
}
