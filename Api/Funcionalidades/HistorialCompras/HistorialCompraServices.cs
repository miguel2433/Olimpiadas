using Api.Persistencia;
using Biblioteca.Dominio;
namespace Api.Funcionalidades.HistorialCompras;

public class HistorialCompraServices : IHistorialCompraServices
{
    private readonly AppDbContext _context;

    public HistorialCompraServices(AppDbContext context)
    {
        _context = context;
    }

    public void AddHistorialCompra(HistorialCompra historialCompra)
    {
        _context.HistorialCompra.Add(historialCompra);
        _context.SaveChanges();
    }

        public void DeleteHistorialCompra(Guid id)
        {
            var historialCompra = _context.HistorialCompra.Find(id);
            if (historialCompra != null)
            {
                _context.HistorialCompra.Remove(historialCompra);
                _context.SaveChanges();
            }
        }

    public List<HistorialCompra> GetHistorialCompra()
    {
        return _context.HistorialCompra.ToList();
    }

        public void UpdateHistorialCompra(Guid id, HistorialCompra historialCompra)
        {
            var historialCompraExistente = _context.HistorialCompra.Find(id);
            if (historialCompraExistente != null)
            {
                historialCompraExistente = historialCompra;
                _context.SaveChanges();
            }
        }
    }

    public interface IHistorialCompraServices
    {
        List<HistorialCompra> GetHistorialCompra();
        void AddHistorialCompra(HistorialCompra historialCompra);
        void UpdateHistorialCompra(Guid id, HistorialCompra historialCompra);
        void DeleteHistorialCompra(Guid id);
    }
}