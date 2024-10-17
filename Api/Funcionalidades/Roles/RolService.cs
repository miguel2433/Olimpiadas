using System;
using System.Security.Claims;
using Api.Persistencia;
using Biblioteca.Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
namespace Api.Funcionalidades.Roles;

public class RolService : IRolService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RolService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public void AddRol(Rol rol)
    {
        _context. Rol.Add(rol);
        _context.SaveChanges();
    }

    public void DeleteRol(Guid id)
    {
        var rol = _context.Rol.Find(id);
        if (rol != null)
        {
            _context.Rol.Remove(rol);
            _context.SaveChanges();
        }
    }
    public object? GetRoles()
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

            if (rol.ToLower() != "admin")
            {
                throw new UnauthorizedAccessException("No tienes permisos para obtener la lista de roles.");
            }

            return _context.Rol.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar el token: {ex.Message}");
            throw new UnauthorizedAccessException($"Error al procesar el token JWT: {ex.Message}");
        }
    }

    public void UpdateRol(Guid id, Rol rol)
    {
        
        var rolExistente = _context.Rol.Find(id);
        if (rolExistente != null)
        {
            rolExistente.Nombre = rol.Nombre;
            rolExistente.Descripcion = rol.Descripcion;
            _context.SaveChanges();
        }
    }
}

public interface IRolService
{
    void AddRol(Rol rol);
    void DeleteRol(Guid id);
    object? GetRoles();
    void UpdateRol(Guid id, Rol rol);
}
