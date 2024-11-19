namespace Api.Funcionalidades.Categorias;
using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

// Esta clase define todos los endpoints relacionados con la gestión de categorías
// Implementa ICarterModule para la configuración de rutas en la API
public class CategoriaEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Configura un grupo de rutas bajo /api/categoria
        var group = app.MapGroup("/api/categoria")
            .WithTags("Categorias");

        // Endpoint para obtener todas las categorías
        group.MapGet("", ([FromServices] ICategoriaService categoriaService) =>
        {
            return Results.Ok(categoriaService.GetCategorias());
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para crear una nueva categoría
        group.MapPost("", ([FromServices] ICategoriaService categoriaService, CategoriaUpdateDto categoria) =>
        {
            categoriaService.AddCategoria(categoria);
            return Results.Ok(categoria);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
        
        // Endpoint para actualizar una categoría existente
        group.MapPut("{id}", ([FromServices] ICategoriaService categoriaService, Guid id, CategoriaUpdateDto categoria) =>
        {
            categoriaService.UpdateCategoria(id, categoria);
            return Results.Ok(categoria);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
        
        // Endpoint para eliminar una categoría
        group.MapDelete("{id}", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            categoriaService.DeleteCategoria(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para buscar categorías por nombre
        group.MapGet("/buscar", ([FromServices] ICategoriaService categoriaService, string nombre) =>
        {
            return Results.Ok(categoriaService.BuscarCategoriasPorNombre(nombre));
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para marcar una categoría como eliminada (soft delete)
        group.MapPut("/{id}/eliminar", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            categoriaService.MarcarCategoriaComoEliminada(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para obtener los productos de una categoría específica
        group.MapGet("/{id}/productos", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            return Results.Ok(categoriaService.ObtenerProductosDeCategoria(id));
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para actualizar la descripción de una categoría
        group.MapPut("/{id}/descripcion", ([FromServices] ICategoriaService categoriaService, Guid id, string descripcion) =>
        {
            categoriaService.ActualizarDescripcionCategoria(id, descripcion);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
