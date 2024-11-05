using Api.Persistencia;
using Biblioteca.Dominio;
namespace Api.Funcionalidades.HistorialPrecios;

public class HistorialPrecioServices : IHistorialPrecioServices
{
    private readonly AppDbContext _context;

    public HistorialPrecioServices(AppDbContext context)
    {
        _context = context;
    }

    public void AddHistorialPrecio(HistorialPrecio historialPrecio)
    {
        _context.HistorialPrecio.Add(historialPrecio);
        _context.SaveChanges();
    }

        public void DeleteHistorialPrecio(int id)
        {
            var historialPrecio = _context.HistorialPrecio.Find(id);
            if (historialPrecio != null)
            {
                _context.HistorialPrecio.Remove(historialPrecio);
                _context.SaveChanges();
            }
        }

    public List<HistorialPrecio> GetHistorialPrecio()
    {
        return _context.HistorialPrecio.ToList();
    }

        public void UpdateHistorialPrecio(int id, HistorialPrecio historialPrecio)
        {
            var historialPrecioExistente = _context.HistorialPrecio.Find(id);
        if (historialPrecioExistente != null)
        {
            historialPrecioExistente = historialPrecio;
            _context.SaveChanges();
        }
    }
}
    
public interface IHistorialPrecioServices
{
    List<HistorialPrecio> GetHistorialPrecio();
    void AddHistorialPrecio(HistorialPrecio historialPrecio);
    void UpdateHistorialPrecio(int id, HistorialPrecio historialPrecio);
    void DeleteHistorialPrecio(int id);
}
