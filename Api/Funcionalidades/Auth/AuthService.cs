using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Biblioteca.Dominio;
using Api.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Api.Funcionalidades.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> Login(string email, string password)
    {
        var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null || !VerifyPassword(password, usuario.Password))
        {
            return null;
        }

        return GenerateJwtToken(usuario);
    }

    private bool VerifyPassword(string inputPassword, string storedPassword)
    {
        // En un escenario real, aquí deberías usar un algoritmo de hash seguro
        return inputPassword == storedPassword;
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT:Key no está configurado"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public interface IAuthService
{
    Task<string> Login(string email, string password);
}
