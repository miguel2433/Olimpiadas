using Api.Persistencia;
using Biblioteca.Dominio;

namespace Api.Funcionalidades.Usuarios;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }   
    public void AddUsuario(Usuario usuario)
    {
        _context.Usuario.Add(usuario);
        _context.SaveChanges();
    }

    public void DeleteUsuario(Guid id)
    {
        var usuario = _context.Usuario.Find(id);
        if (usuario != null)
        {
            _context.Usuario.Remove(usuario);
            _context.SaveChanges();
        }
    }

    public object? GetUsuarios()
    {
        return _context.Usuario.ToList();
    }

    public void UpdateUsuario(Guid id, Usuario usuario)
    {
        var usuarioExistente = _context.Usuario.Find(id);
        if (usuarioExistente != null)
        {
            usuarioExistente = usuario;
            _context.SaveChanges();
        }
    }
}

public interface IUsuarioService
{
    void AddUsuario(Usuario usuario);
    void DeleteUsuario(Guid id);
    object? GetUsuarios();
    void UpdateUsuario(Guid id, Usuario usuario);
}