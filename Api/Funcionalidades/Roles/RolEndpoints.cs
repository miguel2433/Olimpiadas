using Biblioteca.Dominio;
using Carter;
using Api.Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Api.Funcionalidades.Roles;


public class RolEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rol", ([FromServices] IRolService rolService) =>
        {
            return Results.Ok(rolService.GetRoles());
        }).RequireAuthorization();
        app.MapPost("/api/rol", ([FromServices] IRolService rolService, Rol rol) =>
        {
            rolService.AddRol(rol);
            return Results.Ok(rol);
        }).RequireAuthorization();
        app.MapPut("/api/rol/{id}", ([FromServices] IRolService rolService, Guid id, Rol rol) =>
        {
            rolService.UpdateRol(id, rol);
            return Results.Ok(rol);
        }).RequireAuthorization();
        app.MapDelete("/api/rol/{id}", ([FromServices] IRolService rolService, Guid id) =>
        {
            rolService.DeleteRol(id);
            return Results.Ok();
        }).RequireAuthorization();
    }   
}