// Importaciones necesarias para el funcionamiento del servicio
using Api.Persistencia; // Para acceder a la base de datos
using Biblioteca.Dominio; // Para usar las entidades del dominio
using Api.Funcionalidades.Auth; // Para la autenticación

namespace Api.Funcionalidades.HistorialPrecios;

// Clase principal que implementa la interfaz IHistorialPrecioServices
public class HistorialPrecioServices : IHistorialPrecioServices
{
    // Inyección de dependencias
    private readonly AppDbContext _context; // Contexto de base de datos
    private readonly IAuthService _authService; // Servicio de autenticación
    private readonly IHttpContextAccessor _httpContextAccessor; // Acceso al contexto HTTP

    // Constructor que inicializa las dependencias
    public HistorialPrecioServices(AppDbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    // Método para agregar un nuevo historial de precio
    // Solo vendedores y administradores pueden agregar
    public void AddHistorialPrecio(HistorialPrecioDto historialPrecioDto)
    {
        // Verificación de autenticación y autorización
        _authService.AuthenticationVendedoryAdministrador();
        var producto = _context.Producto.Find(historialPrecioDto.ProductoId);
        if(producto == null)
        {
            throw new ArgumentException("Producto no encontrado");
        }
        // Verifica que el usuario sea el vendedor del producto o un administrador
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != producto.VendedorId)
        {
            if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes ver el historial de precios de un producto que no es tuyo");
            }
        }
        // Crea y guarda el nuevo historial de precio
        var historialPrecio = new HistorialPrecio
        {
            ProductoId = historialPrecioDto.ProductoId,
            Precio = historialPrecioDto.Precio,
            FechaCambio = DateTime.Now
        };
        _context.HistorialPrecio.Add(historialPrecio);
        _context.SaveChanges();
    }

    // Método para eliminar un historial de precio
    // Solo el vendedor del producto o un administrador pueden eliminar
    public void DeleteHistorialPrecio(Guid id)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var historialPrecioExistente = _context.HistorialPrecio.Find(id);
        var producto = _context.Producto.Find(historialPrecioExistente.ProductoId);
        if(producto == null)
        {
            throw new ArgumentException("Producto no encontrado");
        }
        // Verifica permisos
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

    // Método para obtener el historial de precios de un producto
    // Solo el vendedor del producto o un administrador pueden ver el historial
    public List<HistorialPrecioGetDto> GetHistorialPrecio(Guid productoId)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var producto = _context.Producto.Find(productoId);
        if(producto == null)
        {
            throw new ArgumentException("Producto no encontrado");
        }
        // Verifica permisos
        if(_authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != producto.VendedorId)
        {
            if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) != "Administrador")
            {
                throw new UnauthorizedAccessException("No puedes ver el historial de precios de un producto que no es tuyo");
            }
        }
        // Retorna lista de historiales convertida a DTO
        return _context.HistorialPrecio.Select(h => new HistorialPrecioGetDto
        {
            Id = h.Id,
            ProductoId = h.ProductoId,
            Precio = h.Precio,
            FechaCambio = h.FechaCambio
        }).Where(h => h.ProductoId == productoId).ToList();
    }

    // Método para actualizar un historial de precio
    // Solo el vendedor del producto o un administrador pueden actualizar
    
    // Explicacion:
    // Este método actualiza un historial de precio existente.
    // Primero, autentica que el usuario sea un vendedor o administrador.
    // Luego, busca el historial de precio existente en la base de datos usando el ID proporcionado.
    // Después, busca el producto asociado a ese historial de precio.
    // Si el producto no se encuentra, lanza una excepción indicando que el producto no fue encontrado.
    public void UpdateHistorialPrecio(Guid id, HistorialPrecioUpdateDto historialPrecioDto)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var historialPrecioExistente = _context.HistorialPrecio.Find(id);
        var producto = _context.Producto.Find(historialPrecioExistente.ProductoId);
        if(producto == null)
        {
            throw new ArgumentException("Producto no encontrado");
        }
        // Verifica permisos
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
    
// Interfaz que define los métodos que debe implementar el servicio
public interface IHistorialPrecioServices
{
    List<HistorialPrecioGetDto> GetHistorialPrecio(Guid productoId); // Obtener historial
    void AddHistorialPrecio(HistorialPrecioDto historialPrecioDto); // Agregar historial
    void UpdateHistorialPrecio(Guid id, HistorialPrecioUpdateDto historialPrecioDto); // Actualizar historial
    void DeleteHistorialPrecio(Guid id); // Eliminar historial
}
