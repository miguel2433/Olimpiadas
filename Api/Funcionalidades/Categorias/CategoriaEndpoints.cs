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
        app.MapPut("/api/categoria/{id}", ([FromServices] ICategoriaService categoriaService, int id, Categoria categoria) =>
        {
            categoriaService.UpdateCategoria(id, categoria);
            return Results.Ok(categoria);
        });
        app.MapDelete("/api/categoria/{id}", ([FromServices] ICategoriaService categoriaService, int id) =>
        {
            categoriaService.DeleteCategoria(id);
            return Results.Ok();
        });
    }
}