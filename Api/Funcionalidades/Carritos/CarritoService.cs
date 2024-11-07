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

    public void AddCarrito(Carrito carrito)
    {
        if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) == "Vendedor")
        {
            throw new UnauthorizedAccessException("No tienes permisos para agregar un carrito");
        }
        _context.Carrito.Add(carrito);
        _context.SaveChanges();
    }

    public void DeleteCarrito(Guid id)
    {
        if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) == "Vendedor")
        {
            throw new UnauthorizedAccessException("No tienes permisos para eliminar un carrito");
        }
        var carrito = _context.Carrito.Find(id);
        if (carrito != null)
        {
            _context.Carrito.Remove(carrito);
            _context.SaveChanges();
        }
    }

    public List<Carrito> GetCarrito()
    {
        return _context.Carrito.ToList();
    }

    public void UpdateCarrito(Guid id, Carrito carrito)
    {
        if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) == "Vendedor")
        {
            throw new UnauthorizedAccessException("No tienes permisos para actualizar un carrito");
        }

        if(carrito.UsuarioId != _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
        {
            throw new UnauthorizedAccessException("No puedes actualizar un carrito que no sea tuyo");
        }
        var carritoExistente = _context.Carrito.Find(id);
        if (carritoExistente != null)
        {
            _context.Carrito.Update(carrito);
            _context.SaveChanges();
        }
    }

    public Carrito? BuscarCarritoPorProducto(Guid productoId)
    {
        return _context.Carrito
            .Include(c => c.Items)
            .ThenInclude(i => i.Producto)
            .FirstOrDefault(c => c.Items.Any(i => i.Producto.Id == productoId));
    }

    public void MarcarComoEntregado(Guid id)//Cambia el estado del carrito a entregado(true).
    {

        var carrito = _context.Carrito.Find(id);
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != carrito.UsuarioId )
        {
            throw new UnauthorizedAccessException("No puedes marcar un carrito como entregado si no es tuyo");
        }

        if (carrito != null)
        {
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
        if (carrito != null)
        {
            carrito.Eliminado = true;
            _context.SaveChanges();
        }
    }

    public List<Carrito> GetCarritoUsuario(Guid id)
    {
        if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) == "Vendedor")
        {
            throw new UnauthorizedAccessException("No tienes permisos para ver los carritos de los usuarios");
        }
        return _context.Carrito.Include(c => c.Usuario).Where(c => c.UsuarioId == id).ToList();
    }

    public Carrito GetCarritoUsuarioId(Guid id)
    {
        var carrito = _context.Carrito.Include(c => c.Usuario).FirstOrDefault(c => c.Id == id);
        if(carrito == null)
        {
            throw new ArgumentException("Carrito no encontrado");
        }
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != carrito.UsuarioId)
        {
            throw new UnauthorizedAccessException("No puedes ver un carrito que no sea tuyo");
        }
        return carrito;
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
        _context.SaveChanges();
    }


}

public interface ICarritoService
{
    void AddCarrito(Carrito carrito);
    void DeleteCarrito(Guid id);
    List<Carrito> GetCarrito();
    void UpdateCarrito(Guid id, Carrito carrito);
    Carrito? BuscarCarritoPorProducto(Guid id);
    void MarcarComoEntregado(Guid id);
    decimal CalcularTotal(Guid id);
    void MarcarComoEliminado(Guid id);
    List<Carrito> GetCarritoUsuario(Guid id);
    Carrito GetCarritoUsuarioId(Guid id);
    void PagarCarrito(Guid id);
}
