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

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUsuarioService _usuarioService;
    public AuthService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IUsuarioService usuarioService)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _usuarioService = usuarioService;
    }

    public async Task<string> Login(string email, string password)
    {
        var usuario = await _context.Usuario.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null || _usuarioService.VerifyPassword(password, usuario.Password))
        {
            return null;
        }

        return GenerateJwtToken(usuario);
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
        var key = Encoding.ASCII.GetBytes(jwt.Key);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre ?? "Usuario")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwt.Issuer,
            Audience = jwt.Audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string ReturnTokenRol(string authorizationHeader)
    {
        if (string.IsNullOrEmpty(authorizationHeader))
        {
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

            return rol;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar el token: {ex.Message}");
            throw new UnauthorizedAccessException($"Error al procesar el token JWT: {ex.Message}");
        }
    }

    public string ReturnTokenId(string authorizationHeader)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(authorizationHeader);
        var idClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameidentifier");
        if (idClaim == null)
        {
            throw new UnauthorizedAccessException("ID no encontrado en el token JWT");
        }
        return idClaim.Value;
    }

}

public interface IAuthService
{
    Task<string> Login(string email, string password);
}
