using Biblioteca.Dominio;
using Microsoft.EntityFrameworkCore;

namespace Api.Funcionalidades.Carritos;

public class CarritoService : ICarritoService
{
    private readonly DbContext _context;

    public CarritoService(DbContext context)
    {
        _context = context;
    }

    public List<Carrito> GetCarrito()
    {
        return _context.Set<Carrito>().ToList();
    }
}

public interface ICarritoService
{
    List<Carrito> GetCarrito();
}