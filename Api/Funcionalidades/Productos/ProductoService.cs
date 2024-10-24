using System.IdentityModel.Tokens.Jwt;
using Api.Persistencia;
using Biblioteca.Dominio;

namespace Api.Funcionalidades.Productos;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductoService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public void AddProducto(Producto producto)
    {
        AuthenticationVendedoryAdministrador();
        _context.Producto.Add(producto);
        _context.SaveChanges();
    }

    public void DeleteProducto(Guid id)
    {
        AuthenticationVendedoryAdministrador();
        var producto = _context.Producto.Find(id);
        if (producto != null)
        {
            _context.Producto.Remove(producto);
            _context.SaveChanges();
        }
    }

    public object? GetProductos()
    {
        return _context.Producto.ToList();
    }

    public void UpdateProducto(Guid id, Producto producto)
    {
        AuthenticationVendedoryAdministrador();
        var productoExistente = _context.Producto.Find(id);
        if (productoExistente != null)
        {
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            _context.SaveChanges();
        }
    }

    private void AuthenticationVendedoryAdministrador()
    {
        
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            Console.WriteLine("Token JWT no proporcionado");
            throw new UnauthorizedAccessException("Token JWT no proporcionado");
        }

        string token;
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorizationHeader.Substring("Bearer ".Length).Trim();
        }
        else
        {
            token = authorizationHeader.Trim();
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            if (!tokenHandler.CanReadToken(token))
            {
                throw new UnauthorizedAccessException("El token proporcionado no es un token JWT válido");
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);


            var rolClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role");

            if (rolClaim == null)
            {
                throw new UnauthorizedAccessException("Rol no encontrado en el token JWT");
            }

            var rol = rolClaim.Value;

            if (rol != "Administrador" || rol != "Vendedor")
            {
                throw new UnauthorizedAccessException("No tienes permisos");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar el token: {ex.Message}");
            throw new UnauthorizedAccessException($"Error al procesar el token JWT: {ex.Message}");
        }
    }
    
}

public interface IProductoService
{
    void AddProducto(Producto producto);
    void DeleteProducto(Guid id);
    object? GetProductos();
    void UpdateProducto(Guid id, Producto producto);
}