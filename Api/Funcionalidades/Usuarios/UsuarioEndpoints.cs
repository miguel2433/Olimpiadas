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
            
            group.MapPost("", ([FromServices] IUsuarioService usuarioService, UsuarioDto usuarioDto, [FromHeader(Name = "Authorization")] string authorizationHeader) =>
            {
                usuarioService.AddUsuario(usuarioDto, authorizationHeader);
                return Results.Ok(usuarioDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPut("{id}", ([FromServices] IUsuarioService usuarioService, Guid id, UsuarioDto usuarioDto, [FromHeader(Name = "Authorization")] string authorizationHeader) =>
            {
                usuarioService.UpdateUsuario(id, usuarioDto, authorizationHeader);
                return Results.Ok(usuarioDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapDelete("{id}", ([FromServices] IUsuarioService usuarioService, Guid id, [FromHeader(Name = "Authorization")] string authorizationHeader) =>
            {
                usuarioService.DeleteUsuario(id, authorizationHeader);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}