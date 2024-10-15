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
    public void AddUsuario(UsuarioDto usuarioDto)
    {
        var rol = _context.Rol.Find(usuarioDto.RolId);
        if (rol == null)
        {
            throw new ArgumentException("Rol no encontrado");
        }
        var usuario = new Usuario
        {
            Nombre = usuarioDto.Nombre,
            NombreUsuario = usuarioDto.NombreUsuario,
            Apellido = usuarioDto.Apellido,
            Email = usuarioDto.Email,
            Password = usuarioDto.Password,
            Telefono = usuarioDto.Telefono,
            Rol = rol
        };
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

    public void UpdateUsuario(Guid id, UsuarioDto usuarioDto)
    {
        var usuarioExistente = _context.Usuario.Find(id);
        var rol = _context.Rol.Find(usuarioDto.RolId);
        if (rol == null)
        {
            throw new ArgumentException("Rol no encontrado");
        }
        if (usuarioExistente != null)
        {
            usuarioExistente.Nombre = usuarioDto.Nombre;
            usuarioExistente.NombreUsuario = usuarioDto.NombreUsuario;
            usuarioExistente.Apellido = usuarioDto.Apellido;
            usuarioExistente.Email = usuarioDto.Email;
            usuarioExistente.Password = usuarioDto.Password;
            usuarioExistente.Telefono = usuarioDto.Telefono;
            usuarioExistente.Rol = rol;
            _context.SaveChanges();
        }
    }
}

public interface IUsuarioService
{
    void AddUsuario(UsuarioDto usuarioDto);
    void DeleteUsuario(Guid id);
    object? GetUsuarios();
    void UpdateUsuario(Guid id, UsuarioDto usuarioDto);
}