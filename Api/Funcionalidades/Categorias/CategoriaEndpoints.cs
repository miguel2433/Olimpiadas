namespace Api.Funcionalidades.Categorias;
using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

public class CategoriaEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categoria", ([FromServices] ICategoriaService categoriaService) =>
        {
            return Results.Ok(categoriaService.GetCategorias());
        });
        app.MapPost("/api/categoria", ([FromServices] ICategoriaService categoriaService, Categoria categoria) =>
        {
            categoriaService.AddCategoria(categoria);
            return Results.Ok(categoria);
        }); 
        app.MapPut("/api/categoria/{id}", ([FromServices] ICategoriaService categoriaService, Guid id, Categoria categoria) =>
        {
            categoriaService.UpdateCategoria(id, categoria);
            return Results.Ok(categoria);
        });
        app.MapDelete("/api/categoria/{id}", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            categoriaService.DeleteCategoria(id);
            return Results.Ok();
        });

        app.MapGet("/api/categoria/buscar", ([FromServices] ICategoriaService categoriaService, string nombre) =>
        {
            return Results.Ok(categoriaService.BuscarCategoriasPorNombre(nombre));
        });

        app.MapPut("/api/categoria/{id}/eliminar", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            categoriaService.MarcarCategoriaComoEliminada(id);
            return Results.Ok();
        });

        app.MapGet("/api/categoria/{id}/productos", ([FromServices] ICategoriaService categoriaService, Guid id) =>
        {
            return Results.Ok(categoriaService.ObtenerProductosDeCategoria(id));
        });

        app.MapPut("/api/categoria/{id}/descripcion", ([FromServices] ICategoriaService categoriaService, Guid id, string descripcion) =>
        {
            categoriaService.ActualizarDescripcionCategoria(id, descripcion);
            return Results.Ok();
        });
    }
}
