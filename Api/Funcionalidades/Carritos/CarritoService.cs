using Api.Persistencia;
using Biblioteca.Dominio;
using Microsoft.EntityFrameworkCore;
using Api.Funcionalidades.Auth;
namespace Api.Funcionalidades.Carritos;

// Esta clase implementa la lógica de negocio relacionada con los carritos
// Gestiona todas las operaciones CRUD y validaciones de carritos
public class CarritoService : ICarritoService
{
    // Dependencias inyectadas para acceso a datos y servicios de autenticación
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Constructor que inicializa las dependencias
    public CarritoService(AppDbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    // Elimina un carrito si el usuario tiene permisos
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

    // Obtiene los carritos del usuario actual o todos si es administrador
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

    // Busca carritos que contengan un producto específico
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

    // Marca un carrito como entregado si el usuario tiene permisos
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

    // Calcula el total del carrito sumando los subtotales de sus items
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

    // Marca un carrito como eliminado (soft delete)
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

    // Procesa el pago del carrito y actualiza el stock de productos
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
            Console.WriteLine(item.Producto.Id);
            var producto = item.Producto;
            if(producto == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }
            if(producto.Stock < item.Cantidad)
            {
                throw new ArgumentException("Stock insuficiente");
            }
            Console.WriteLine(producto.Stock);
            producto.Stock -= item.Cantidad;
            Console.WriteLine(producto.Stock);
        }
        carrito.Total = CalcularTotal(id);
        _context.SaveChanges();
    }

}

// Interfaz que define los métodos que debe implementar el servicio de carritos
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
