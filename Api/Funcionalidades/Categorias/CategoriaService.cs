using Api.Persistencia;
using Biblioteca.Dominio;
using Api.Funcionalidades.Auth;
namespace Api.Funcionalidades.Categorias;

// Esta clase implementa la lógica de negocio para la gestión de categorías
public class CategoriaService : ICategoriaService
{
    // Dependencias inyectadas para acceso a datos y servicios de autenticación
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    // Constructor que inicializa las dependencias necesarias
    public CategoriaService(AppDbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    // Agrega una nueva categoría (solo administradores)
    public void AddCategoria(CategoriaUpdateDto categoria)
    {
        _authService.AuthenticationAdmin();
        var categoriaEntity = new Categoria
        {
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };
        
        _context.Categoria.Add(categoriaEntity);
        _context.SaveChanges();
    }

    // Elimina permanentemente una categoría (solo administradores)
    public void DeleteCategoria(Guid id)
    {
        _authService.AuthenticationAdmin();
        var categoria = _context.Categoria.Find(id);
        if (categoria != null)
        {
            _context.Categoria.Remove(categoria);
            _context.SaveChanges();
        }
    }

    // Obtiene todas las categorías
    // Los administradores ven todas, incluidas las eliminadas
    // Los usuarios normales solo ven las categorías activas
    public List<CategoriaDto> GetCategorias()
    {
        if(_authService.ReturnTokenRol(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()) == "Administrador")
        {
            return _context.Categoria.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Productos = c.Productos.Select(p => p.Id).ToList(),
                Eliminado = c.Eliminado
            }).ToList();
        }
        return _context.Categoria.Where(c => !c.Eliminado).Select(c => new CategoriaDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion,
            Productos = c.Productos.Select(p => p.Id).ToList()
        }).ToList();
    }

    // Actualiza los datos de una categoría existente (solo administradores)
    public void UpdateCategoria(Guid id, CategoriaUpdateDto categoria)
    {
        _authService.AuthenticationAdmin();
        var categoriaExistente = _context.Categoria.Find(id);
        if (categoriaExistente != null)
        {
            categoriaExistente.Nombre = categoria.Nombre;
            if(categoria.Descripcion != null)
            {
                categoriaExistente.Descripcion = categoria.Descripcion;
            }
            _context.SaveChanges();
        }
    }

    // Busca categorías que contengan el nombre especificado
    public List<Categoria> BuscarCategoriasPorNombre(string nombre)
    {
        return _context.Categoria.Where(c => c.Nombre.Contains(nombre)).ToList();
    }

    // Marca una categoría como eliminada (soft delete)
    public void MarcarCategoriaComoEliminada(Guid id)
    {
        var categoria = _context.Categoria.Find(id);
        if (categoria != null)
        {
            categoria.Eliminado = true;
            _context.SaveChanges();
        }
    }

    // Obtiene todos los productos asociados a una categoría
    public List<Producto> ObtenerProductosDeCategoria(Guid id)
    {
        var categoria = _context.Categoria.FirstOrDefault(c => c.Id == id);
        return categoria?.Productos ?? new List<Producto>();
    }

    // Actualiza la descripción de una categoría (solo administradores)
    public void ActualizarDescripcionCategoria(Guid id, string descripcion)
    {
        _authService.AuthenticationAdmin();
        var categoria = _context.Categoria.Find(id);
        if (categoria != null)
        {
            categoria.Descripcion = descripcion;
            _context.SaveChanges();
        }
    }
    
}

// Interfaz que define los métodos que debe implementar el servicio de categorías
public interface ICategoriaService
{
    void AddCategoria(CategoriaUpdateDto categoria);
    void UpdateCategoria(Guid id, CategoriaUpdateDto categoria);
    void DeleteCategoria(Guid id);
    List<CategoriaDto> GetCategorias();
    List<Categoria> BuscarCategoriasPorNombre(string nombre);
    void MarcarCategoriaComoEliminada(Guid id);
    List<Producto> ObtenerProductosDeCategoria(Guid id);
    void ActualizarDescripcionCategoria(Guid id, string descripcion);
}
