// Importaciones necesarias para el manejo de tokens JWT, claims y otras funcionalidades
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Biblioteca.Dominio;
using Api.Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Api.Funcionalidades.Usuarios;

namespace Api.Funcionalidades.Auth;

// Servicio de autenticación que implementa la interfaz IAuthService
public class AuthService : IAuthService
{
    private readonly AppDbContext _context; // Contexto de base de datos
    private readonly IConfiguration _configuration; // Configuración de la aplicación
    private readonly IHttpContextAccessor _httpContextAccessor; // Acceso al contexto HTTP

    // Constructor que inyecta las dependencias necesarias
    public AuthService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    // Método para verificar si la contraseña ingresada coincide con el hash almacenado
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    // Método de inicio de sesión que valida las credenciales y genera un token JWT
    public async Task<string> Login(LoginRequest loginRequest)
    {
        // Busca el usuario por email incluyendo su rol
        var usuario = await _context.Usuario.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

        // Verifica si el usuario existe y si la contraseña es correcta
        if (usuario == null || !VerifyPassword(loginRequest.Password, usuario.Password))
        {
            return null;
        }

        return GenerateJwtToken(usuario);
    }

    // Método privado para generar el token JWT
    private string GenerateJwtToken(Usuario usuario)
    {
        var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
        var key = Encoding.ASCII.GetBytes(jwt.Key);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Configuración de los claims del token
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre ?? "Usuario")
            }),
            Expires = DateTime.UtcNow.AddDays(7), // Token válido por 7 días
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwt.Issuer,
            Audience = jwt.Audience
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Método para extraer el rol del token JWT
    public string ReturnTokenRol(string authorizationHeader)
    {
        // Verifica si se proporcionó un token
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            throw new UnauthorizedAccessException("Token JWT no proporcionado");
        }

        // Extrae el token del header de autorización
        string token;
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorizationHeader.Substring("Bearer ".Length).Trim();
        }
        else
        {
            token = authorizationHeader.Trim();
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            // Verifica si el token es válido
            if (!tokenHandler.CanReadToken(token))
            {
                throw new UnauthorizedAccessException("El token proporcionado no es un token JWT válido");
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Busca el claim de rol en el token
            var rolClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role");

            if (rolClaim == null)
            {
                throw new UnauthorizedAccessException("Rol no encontrado en el token JWT");
            }

            var rol = rolClaim.Value;

            return rol;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar el token: {ex.Message}");
            throw new UnauthorizedAccessException($"Error al procesar el token JWT: {ex.Message}");
        }
    }

    public Guid ReturnTokenId(string authorizationHeader)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(authorizationHeader);
        var idClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameidentifier");
        if (idClaim == null)
        {
            throw new UnauthorizedAccessException("ID no encontrado en el token JWT");
        }
        return Guid.Parse(idClaim.Value);
    }

    public void AuthenticationAdmin()
    {
        
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            Console.WriteLine("Token JWT no proporcionado");
            throw new UnauthorizedAccessException("Token JWT no proporcionado");
        }

        string token;
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorizationHeader.Substring("Bearer ".Length).Trim();
        }
        else
        {
            token = authorizationHeader.Trim();
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            if (!tokenHandler.CanReadToken(token))
            {
                throw new UnauthorizedAccessException("El token proporcionado no es un token JWT válido");
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);


            var rolClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role");

            if (rolClaim == null)
            {
                throw new UnauthorizedAccessException("Rol no encontrado en el token JWT");
            }

            var rol = rolClaim.Value;

            if (rol != "Administrador")
            {
                throw new UnauthorizedAccessException("No tienes permisos");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar el token: {ex.Message}");
            throw new UnauthorizedAccessException($"Error al procesar el token JWT: {ex.Message}");
        }
    }

    public void AuthenticationVendedoryAdministrador()
    {
        
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            Console.WriteLine("Token JWT no proporcionado");
            throw new UnauthorizedAccessException("Token JWT no proporcionado");
        }

        string token;
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorizationHeader.Substring("Bearer ".Length).Trim();
        }
        else
        {
            token = authorizationHeader.Trim();
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            if (!tokenHandler.CanReadToken(token))
            {
                throw new UnauthorizedAccessException("El token proporcionado no es un token JWT válido");
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);


            var rolClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role");

            if (rolClaim == null)
            {
                throw new UnauthorizedAccessException("Rol no encontrado en el token JWT");
            }

            var rol = rolClaim.Value;

            if (rol != "Administrador" || rol != "Vendedor")
            {
                throw new UnauthorizedAccessException("No tienes permisos");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar el token: {ex.Message}");
            throw new UnauthorizedAccessException($"Error al procesar el token JWT: {ex.Message}");
        }
    }

    public bool VerificarUsuarioActual(Guid id)
    {
        var idActual = ReturnTokenId(_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString());
        if (idActual != id)
        {
            return false;
        }
        return true;
    }
}

// Interfaz que define los métodos que debe implementar el servicio de autenticación
public interface IAuthService
{
    Task<string> Login(LoginRequest authDto);
    Guid ReturnTokenId(string authorizationHeader);
    string ReturnTokenRol(string authorizationHeader);
    void AuthenticationAdmin();
    void AuthenticationVendedoryAdministrador();
    bool VerificarUsuarioActual(Guid id);
}
