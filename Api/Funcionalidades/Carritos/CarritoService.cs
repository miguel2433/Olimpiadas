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

    public void DeleteCarrito(int id)
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

    public void UpdateCarrito(int id, Carrito carrito)
    {
        var carritoExistente = _context.Carrito.Find(id);
        if (carritoExistente != null)
        {
            _context.Carrito.Update(carrito);
            _context.SaveChanges();
        }
    }
}

public interface ICarritoService
{
    void AddCarrito(Carrito carrito);
    void DeleteCarrito(int id);
    List<Carrito> GetCarrito();
    void UpdateCarrito(int id, Carrito carrito);
}