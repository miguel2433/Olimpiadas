using Api.Persistencia;
using Biblioteca.Dominio;
using Api.Funcionalidades.Auth;
using System.Security.Claims;
using BCrypt.Net;

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
        var contraHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password);
        if(contra == "123456")
        {
            var usuario = new Usuario
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
            var usuario = new Usuario
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
            var usuario = new Usuario
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
        var id = _authService.ReturnTokenId(token);
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
                if (id == usuario.Id)
                {
                    GetUsuario(id);
                    usuario.Eliminar = true;
                    _context.SaveChanges();
                }
                break;
            case "Usuario":
                if (id == usuario.Id)
                {
                    GetUsuario(id);
                    usuario.Eliminar = true;
                    _context.SaveChanges();
                }
                break;
        }
    }

    public object? GetUsuarios()
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
    object? GetUsuarios();
    void UpdateUsuario(Guid id, UsuarioDto usuarioDto, string token);
    bool VerifyPassword(string password, string passwordHash);
}