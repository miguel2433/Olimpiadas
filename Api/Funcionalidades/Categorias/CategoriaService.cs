using Api.Persistencia;
using Biblioteca.Dominio;

namespace Api.Funcionalidades.Categorias;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public void AddCategoria(Categoria categoria)
    {
        _context.Categoria.Add(categoria);
        _context.SaveChanges();
    }

    public void DeleteCategoria(Guid id)
    {
        var categoria = _context.Categoria.Find(id);
        if (categoria != null)
        {
            _context.Categoria.Remove(categoria);
            _context.SaveChanges();
        }
    }

    public List<Categoria> GetCategorias()
    {
        return _context.Categoria.ToList();
    }

    public void UpdateCategoria(Guid id, Categoria categoria)
    {
        var categoriaExistente = _context.Categoria.Find(id);
        if (categoriaExistente != null)
        {
            _context.Categoria.Update(categoria);
            _context.SaveChanges();
        }
    }

    public List<Categoria> BuscarCategoriasPorNombre(string nombre)
    {
        return _context.Categoria.Where(c => c.Nombre.Contains(nombre)).ToList();
    }

    public void MarcarCategoriaComoEliminada(Guid id)
    {
        var categoria = _context.Categoria.Find(id);
        if (categoria != null)
        {
            categoria.Eliminado = true;
            _context.SaveChanges();
        }
    }

    public List<Producto> ObtenerProductosDeCategoria(Guid id)
    {
        var categoria = _context.Categoria.FirstOrDefault(c => c.Id == id);
        return categoria?.Productos ?? new List<Producto>();
    }

    public void ActualizarDescripcionCategoria(Guid id, string descripcion)
    {
        var categoria = _context.Categoria.Find(id);
        if (categoria != null)
        {
            categoria.Descripcion = descripcion;
            _context.SaveChanges();
        }
    }
    
}

public interface ICategoriaService
{
    void AddCategoria(Categoria categoria);
    void UpdateCategoria(Guid id, Categoria categoria);
    void DeleteCategoria(Guid id);
    List<Categoria> GetCategorias();
    List<Categoria> BuscarCategoriasPorNombre(string nombre);
    void MarcarCategoriaComoEliminada(Guid id);
    List<Producto> ObtenerProductosDeCategoria(Guid id);
    void ActualizarDescripcionCategoria(Guid id, string descripcion);
}
