using Api.Persistencia;
using Biblioteca.Dominio;

namespace Api.Funcionalidades.Productos;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public void AddProducto(Producto producto)
    {
        _context.Producto.Add(producto);
        _context.SaveChanges();
    }

    public void DeleteProducto(int id)
    {
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

    public void UpdateProducto(int id, Producto producto)
    {
        var productoExistente = _context.Producto.Find(id);
        if (productoExistente != null)
        {
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            _context.SaveChanges();
        }
    }
}

public interface IProductoService
{
    void AddProducto(Producto producto);
    void DeleteProducto(int id);
    object? GetProductos();
    void UpdateProducto(int id, Producto producto);
}