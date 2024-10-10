using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.Usuarios;

public class UsuarioEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/usuario", ([FromServices] IUsuarioService usuarioService) =>
        {
            return Results.Ok(usuarioService.GetUsuarios());
        });
        
        app.MapPost("/api/usuario", ([FromServices] IUsuarioService usuarioService, Usuario usuario) =>
        {
            usuarioService.AddUsuario(usuario);
            return Results.Ok(usuario);
        });
        
        app.MapPut("/api/usuario/{id}", ([FromServices] IUsuarioService usuarioService, int id, Usuario usuario) =>
        {
            usuarioService.UpdateUsuario(id, usuario);
            return Results.Ok(usuario);
        });
        
        app.MapDelete("/api/usuario/{id}", ([FromServices] IUsuarioService usuarioService, int id) =>
        {
            usuarioService.DeleteUsuario(id);
            return Results.Ok();
        });
    }
}