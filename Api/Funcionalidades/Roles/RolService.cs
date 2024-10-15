using System;
using Api.Persistencia;
using Biblioteca.Dominio;

namespace Api.Funcionalidades.Roles;

public class RolService : IRolService
{
    private readonly AppDbContext _context;

    public RolService(AppDbContext context)
    {
        _context = context;
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
        return _context.Rol.ToList();
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

