using Api.Persistencia;
using Biblioteca.Dominio;
using Api.Funcionalidades.Auth;
using Microsoft.EntityFrameworkCore;

namespace Api.Funcionalidades.Usuarios;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;

    public UsuarioService(AppDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    public void AddUsuario(UsuarioDto usuarioDto, string? contra)
    {   
        var usuario = new Usuario();
        var contraHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password);
        if(contra == "123456")
        {
            usuario = new Usuario
            {
                Nombre = usuarioDto.Nombre,
                NombreUsuario = usuarioDto.NombreUsuario,
                Apellido = usuarioDto.Apellido,
                Email = usuarioDto.Email,
                Password = contraHash,
                Telefono = usuarioDto.Telefono,
                Rol = _context.Rol.FirstOrDefault(r => r.Nombre == "Vendedor")
            };
        }
        else if(contra == "654321")
        {
            usuario = new Usuario
            {
                Nombre = usuarioDto.Nombre,
                NombreUsuario = usuarioDto.NombreUsuario,
                Apellido = usuarioDto.Apellido,
                Email = usuarioDto.Email,
                Password = contraHash,
                Rol = _context.Rol.FirstOrDefault(r => r.Nombre == "Administrador")
            };
        }
        else
        {
            usuario = new Usuario
            {
                Nombre = usuarioDto.Nombre,
                NombreUsuario = usuarioDto.NombreUsuario,
                Apellido = usuarioDto.Apellido,
                Email = usuarioDto.Email,
                Password = contraHash,
                Rol = _context.Rol.FirstOrDefault(r => r.Nombre == "Usuario")
            };
        }
        _context.Usuario.Add(usuario);
        _context.SaveChanges();
    }

    public void DeleteUsuario(Guid id, string token)
    {
        var usuario = _context.Usuario.Find(id);
        var rol = _authService.ReturnTokenRol(token);
        if(usuario == null)
        {
            throw new ArgumentException("Usuario no encontrado");
        }
        switch (rol)
        {
            case "Administrador":
                _context.Usuario.Remove(usuario);
                _context.SaveChanges();
                break;
            case "Vendedor":
                if (_authService.VerificarUsuarioActual(id))
                {
                    GetUsuario(id);
                    usuario.Eliminado = true;
                    _context.SaveChanges();
                }
                break;
            case "Usuario":
                if (_authService.VerificarUsuarioActual(id))
                {
                    GetUsuario(id);
                    usuario.Eliminado = true;
                    _context.SaveChanges();
                }
                break;
        }
    }

    public List<Usuario> GetUsuarios()
    {
        return _context.Usuario.ToList();
    }

    public void UpdateUsuario(Guid id, UsuarioDto usuarioDto, string token)
    {
        var usuarioExistente = _context.Usuario.Include(u => u.Rol).FirstOrDefault(u => u.Id == id);

        var tokenRol = _authService.ReturnTokenRol(token);
        if (tokenRol == null)
        {
            throw new ArgumentException("Rol no encontrado");
        }
        if (usuarioExistente != null)
        {
            if(tokenRol == "Usuario" || tokenRol == "Vendedor")
            {
                if(!_authService.VerificarUsuarioActual(id))
                {
                    throw new UnauthorizedAccessException("No puedes modificar este usuario");
                }
            }
            usuarioExistente.Nombre = usuarioDto.Nombre;
            usuarioExistente.NombreUsuario = usuarioDto.NombreUsuario;
            usuarioExistente.Apellido = usuarioDto.Apellido;
            usuarioExistente.Email = usuarioDto.Email;
            usuarioExistente.Password = usuarioDto.Password;
            usuarioExistente.Telefono = usuarioDto.Telefono;
            _context.SaveChanges();
        }
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    public object? GetUsuario(Guid id)
    {
        return _context.Usuario.Find(id);
    }
}

public interface IUsuarioService
{
    void AddUsuario(UsuarioDto usuarioDto, string? contra);
    void DeleteUsuario(Guid id, string token);
    List<Usuario> GetUsuarios();
    void UpdateUsuario(Guid id, UsuarioDto usuarioDto, string token);
    bool VerifyPassword(string password, string passwordHash);
}