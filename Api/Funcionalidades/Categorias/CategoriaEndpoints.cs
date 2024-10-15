namespace Api.Funcionalidades.Categorias;
using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;


public class CategoriaEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categoria")
            .WithTags("Categorias");
        group.MapGet("", ([FromServices] ICategoriaService categoriaService) =>
        {
            return Results.Ok(categoriaService.GetCategorias());
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("", ([FromServices] ICategoriaService categoriaService, Categoria categoria) =>
        {
            categoriaService.AddCategoria(categoria);
            return Results.Ok(categoria);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapPut("{id}", ([FromServices] ICategoriaService categoriaService, Guid id, Categoria categoria) =>
        {
            categoriaService.UpdateCategoria(id, categoria);
            return Results.Ok(categoria);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
        
        group.MapDelete("{id}", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            categoriaService.DeleteCategoria(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/buscar", ([FromServices] ICategoriaService categoriaService, string nombre) =>
        {
            return Results.Ok(categoriaService.BuscarCategoriasPorNombre(nombre));
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/{id}/eliminar", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            categoriaService.MarcarCategoriaComoEliminada(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/{id}/productos", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            return Results.Ok(categoriaService.ObtenerProductosDeCategoria(id));
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/{id}/descripcion", ([FromServices] ICategoriaService categoriaService, Guid id, string descripcion) =>
        {
            categoriaService.ActualizarDescripcionCategoria(id, descripcion);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
