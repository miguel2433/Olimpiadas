using Api.Persistencia;
using Biblioteca.Dominio;
using Api.Funcionalidades.Auth;

namespace Api.Funcionalidades.HistorialPrecios;

public class HistorialPrecioServices : IHistorialPrecioServices
{
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HistorialPrecioServices(AppDbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public void AddHistorialPrecio(HistorialPrecioDto historialPrecioDto)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var producto = _context.Producto.Find(historialPrecioDto.ProductoId);
        if(producto == null)
        {
            throw new ArgumentException("Producto no encontrado");
        }
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != producto.VendedorId)
        {
            if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes ver el historial de precios de un producto que no es tuyo");
            }
        }
        var historialPrecio = new HistorialPrecio
        {
            ProductoId = historialPrecioDto.ProductoId,
            Precio = historialPrecioDto.Precio,
            FechaCambio = DateTime.Now
        };
        _context.HistorialPrecio.Add(historialPrecio);
        _context.SaveChanges();
    }

    public void DeleteHistorialPrecio(Guid id)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var historialPrecioExistente = _context.HistorialPrecio.Find(id);
        var producto = _context.Producto.Find(historialPrecioExistente.ProductoId);
        if(producto == null)
        {
            throw new ArgumentException("Producto no encontrado");
        }
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != producto.VendedorId)
        {
            if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes ver el historial de precios de un producto que no es tuyo");
            }
        }
        if (historialPrecioExistente != null)
        {
            _context.HistorialPrecio.Remove(historialPrecioExistente);
            _context.SaveChanges();
        }
    }

    public List<HistorialPrecioGetDto> GetHistorialPrecio(Guid productoId)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var producto = _context.Producto.Find(productoId);
        if(producto == null)
        {
            throw new ArgumentException("Producto no encontrado");
        }
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != producto.VendedorId)
        {
            if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes ver el historial de precios de un producto que no es tuyo");
            }
        }
        return _context.HistorialPrecio.Select(h => new HistorialPrecioGetDto
        {
            Id = h.Id,
            ProductoId = h.ProductoId,
            Precio = h.Precio,
            FechaCambio = h.FechaCambio
        }).Where(h => h.ProductoId == productoId).ToList();
    }

    public void UpdateHistorialPrecio(Guid id, HistorialPrecioUpdateDto historialPrecioDto)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var historialPrecioExistente = _context.HistorialPrecio.Find(id);
        var producto = _context.Producto.Find(historialPrecioExistente.ProductoId);
        if(producto == null)
        {
            throw new ArgumentException("Producto no encontrado");
        }
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != producto.VendedorId)
        {
            if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes ver el historial de precios de un producto que no es tuyo");
            }
        }
        if (historialPrecioExistente != null)
        {
            historialPrecioExistente.Precio = historialPrecioDto.Precio;
            historialPrecioExistente.FechaCambio = DateTime.Now;
            _context.SaveChanges();
        }
    }
}
    
public interface IHistorialPrecioServices
{
    List<HistorialPrecioGetDto> GetHistorialPrecio(Guid productoId);
    void AddHistorialPrecio(HistorialPrecioDto historialPrecioDto);
    void UpdateHistorialPrecio(Guid id, HistorialPrecioUpdateDto historialPrecioDto);
    void DeleteHistorialPrecio(Guid id);
}
