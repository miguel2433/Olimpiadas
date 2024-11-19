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

    public void AddProducto(ProductoPostDto productoDto)
    {
        _authService.AuthenticationVendedoryAdministrador();

        var producto = new Producto();

        var vendedorId = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        
        producto.Nombre = productoDto.Nombre;
        producto.Stock = productoDto.Stock;
        producto.UrlImagen = productoDto.UrlImagen;
        producto.VendedorId = vendedorId;
        
        foreach(var categoriaId in productoDto.CategoriaIds)
        {
            var categoria = _context.Categoria.Find(categoriaId);
            if(categoria != null)
            {
                producto.Categorias.Add(categoria);
            }
            else
            {
                throw new Exception("Categoria no encontrada");
            }
        }

        var historialPrecio = new HistorialPrecio();
        historialPrecio.Precio = productoDto.Precio;
        historialPrecio.FechaCambio = DateTime.Now;
        
        producto.HistorialPrecios.Add(historialPrecio);
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

            if(idActual != null)
            {
                if(producto.VendedorId == idActual)
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

    public void UpdateProducto(ProductoPutDto productoDto)
    {
        _authService.AuthenticationVendedoryAdministrador();
        var vendedorActual = _authService.ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        var productoExistente = _context.Producto.Find(productoDto.Id);
        if (productoExistente != null)
        {
            if(vendedorActual != null)
            {
                if(productoExistente.VendedorId == vendedorActual || _authService.ReturnTokenRol(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()) == "Administrador")
                {
                    productoExistente.Nombre = productoDto.Nombre;
                    productoExistente.Stock = productoDto.Stock;
                    productoExistente.UrlImagen = productoDto.UrlImagen;
                    _context.SaveChanges();
                }
            }
        }
    }
    
}

public interface IProductoService
{
    void AddProducto(ProductoPostDto productoDto);
    void DeleteProducto(Guid id);
    object? GetProductos();
    void UpdateProducto(ProductoPutDto productoDto);
}