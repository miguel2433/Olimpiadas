using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.Productos
{
    public class ProductoEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/producto")
                .WithTags("Productos");

            group.MapGet("", ([FromServices] IProductoService productoService) =>
            {
                return Results.Ok(productoService.GetProductos());
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPost("", ([FromServices] IProductoService productoService, ProductoPostDto productoDto) =>
            {
                productoService.AddProducto(productoDto);
                return Results.Ok(productoDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPut("", ([FromServices] IProductoService productoService, ProductoPutDto productoDto) =>
            {
                productoService.UpdateProducto(productoDto);
                return Results.Ok(productoDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapDelete("{id}", ([FromServices] IProductoService productoService, Guid id) =>
            {
                productoService.DeleteProducto(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}