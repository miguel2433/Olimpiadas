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
        var group = app.MapGroup("/api/rol")
            .WithTags("Roles");

        group.MapGet("",([FromServices] IRolService rolService) =>
        {
            return Results.Ok(rolService.GetRoles());
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("", ([FromServices] IRolService rolService, Rol rol) =>
        {
            rolService.AddRol(rol);
            return Results.Ok(rol);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("{id}", ([FromServices] IRolService rolService, Guid id, Rol rol) =>
        {
            rolService.UpdateRol(id, rol);
            return Results.Ok(rol);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapDelete("{id}", ([FromServices] IRolService rolService, Guid id) =>
        {
            rolService.DeleteRol(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
