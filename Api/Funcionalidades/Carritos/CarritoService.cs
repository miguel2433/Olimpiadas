using Api.Persistencia;
using Biblioteca.Dominio;
using Microsoft.EntityFrameworkCore;

namespace Api.Funcionalidades.Carritos;

public class CarritoService : ICarritoService
{
    private readonly AppDbContext _context;
    public CarritoService(AppDbContext context)
    {
        _context = context;
    }

    public void AddCarrito(Carrito carrito)
    {
        _context.Carrito.Add(carrito);
        _context.SaveChanges();
    }

    public void DeleteCarrito(Guid id)
    {
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
            .Include(c => c.Productos)
            .FirstOrDefault(c => c.Productos.Any(p => p.Id == productoId));
    }

    public void MarcarComoEntregado(Guid id)//Cambia el estado del carrito a entregado(true).
    {
        var carrito = _context.Carrito.Find(id);
        if (carrito != null)
        {
            carrito.Entregado = true;
            _context.SaveChanges();
        }
    }

    public decimal CalcularTotal(Guid id)
    {
        var carrito = _context.Carrito
            .Include(c => c.Productos)
            .FirstOrDefault(c => c.Id == id);
        return carrito?.Productos.Sum(p => p.Precio) ?? 0;
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
}
