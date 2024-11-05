using System.IdentityModel.Tokens.Jwt;
using Api.Persistencia;
using Biblioteca.Dominio;
using Api.Funcionalidades.Auth;
namespace Api.Funcionalidades.Productos;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthService _authService;

    public ProductoService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IAuthService authService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _authService = authService;
    }

    public void AddProducto(Producto producto)
    {
        _authService.AuthenticationVendedoryAdministrador();
        producto.VendedorId = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        _context.Producto.Add(producto);
        _context.SaveChanges();
    }

    public void DeleteProducto(Guid id)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var idActual = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());

        var producto = _context.Producto.Find(id);
        if (producto != null)
        {

            if(vendedorActual != null)
            {
                if(producto.VendedorId == vendedorActual)
                {
                    producto.Eliminado = true;
                    _context.SaveChanges();
                }
                else if( _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) == "Administrador")
                {
                    _context.Producto.Remove(producto);
                    _context.SaveChanges();
                }
            }
        }
    }

    public object? GetProductos()
    {
        return _context.Producto.ToList();
    }

    public void UpdateProducto(Guid id, Producto producto)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var vendedorActual = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        var productoExistente = _context.Producto.Find(id);
        if (productoExistente != null)
        {
            if(vendedorActual != null)
            {
                if(productoExistente.VendedorId == vendedorActual || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) == "Administrador")
                {
                    productoExistente.Nombre = producto.Nombre;
                    productoExistente.Stock = producto.Stock;
                    _context.SaveChanges();
                }
            }
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