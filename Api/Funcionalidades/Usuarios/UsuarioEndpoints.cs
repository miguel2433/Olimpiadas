using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.Usuarios
{
    public class UsuarioEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/usuario")
                .WithTags("Usuarios");

            group.MapGet("", ([FromServices] IUsuarioService usuarioService) =>
            {
                return Results.Ok(usuarioService.GetUsuarios());
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPost("", ([FromServices] IUsuarioService usuarioService, UsuarioDto usuarioDto) =>
            {
                usuarioService.AddUsuario(usuarioDto);
                return Results.Ok(usuarioDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPut("{id}", ([FromServices] IUsuarioService usuarioService, Guid id, UsuarioDto usuarioDto) =>
            {
                usuarioService.UpdateUsuario(id, usuarioDto);
                return Results.Ok(usuarioDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapDelete("{id}", ([FromServices] IUsuarioService usuarioService, Guid id) =>
            {
                usuarioService.DeleteUsuario(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}