using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.Productos
{
    public class ProductoEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/producto", ([FromServices] IProductoService productoService) =>
            {
                return Results.Ok(productoService.GetProductos());
            });
            
            app.MapPost("/api/producto", ([FromServices] IProductoService productoService, Producto producto) =>
            {
                productoService.AddProducto(producto);
                return Results.Ok(producto);
            });
            
            app.MapPut("/api/producto/{id}", ([FromServices] IProductoService productoService, Guid id, Producto producto) =>
            {
                productoService.UpdateProducto(id, producto);
                return Results.Ok(producto);
            });
            
            app.MapDelete("/api/producto/{id}", ([FromServices] IProductoService productoService, Guid id) =>
            {
                productoService.DeleteProducto(id);
                return Results.Ok();
            });
        }
    }
}