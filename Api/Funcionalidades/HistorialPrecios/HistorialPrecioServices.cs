using Api.Persistencia;
using Biblioteca.Dominio;
namespace Api.Funcionalidades.HistorialPrecios;

public class HistorialPrecioServices : IHistorialPrecioServices
{
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;

    public HistorialPrecioServices(AppDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public void AddHistorialPrecio(HistorialPrecio historialPrecio)
    {
        _authService.AuthenticationVendedoryAdministrador();
        _context.HistorialPrecio.Add(historialPrecio);
        _context.SaveChanges();
    }

    public void DeleteHistorialPrecio(int id)
    {
        _authService.AuthenticationVendedoryAdministrador();
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
        _authService.AuthenticationVendedoryAdministrador();
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
