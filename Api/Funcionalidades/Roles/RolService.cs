using System;
using System.Security.Claims;
using Api.Persistencia;
using Biblioteca.Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Api.Funcionalidades.Auth;

namespace Api.Funcionalidades.Roles;

public class RolService : IRolService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthService _authService;

    public RolService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IAuthService authService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _authService = authService;
    }

    public void AddRol(Rol rol)
    {
        _authService.AuthenticationAdmin();
        _context.Rol.Add(rol);
        _context.SaveChanges();
    }

    public void DeleteRol(Guid id)
    {
        _authService.AuthenticationAdmin();
        var rol = _context.Rol.Find(id);
        if (rol != null)
        {
            _context.Rol.Remove(rol);
            _context.SaveChanges();
        }
    }
    public object? GetRoles()
    {
        _authService.AuthenticationAdmin();
        return _context.Rol.ToList();
    }

    public void UpdateRol(Guid id, Rol rol)
    {
        _authService.AuthenticationAdmin();
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
