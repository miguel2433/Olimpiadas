using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.Productos
{
    /// <summary>
    /// Clase que define los endpoints de la API para el manejo de productos
    /// </summary>
    public class ProductoEndpoints : ICarterModule
    {
        /// <summary>
        /// Configura las rutas y endpoints disponibles para productos
        /// </summary>
        /// <param name="app">Constructor de rutas de la aplicación</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Crea un grupo de rutas con el prefijo /api/producto
            var group = app.MapGroup("/api/producto")
                .WithTags("Productos");

            // GET: Obtiene todos los productos
            group.MapGet("", ([FromServices] IProductoService productoService) =>
            {
                return Results.Ok(productoService.GetProductos());
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            // POST: Crea un nuevo producto
            group.MapPost("", ([FromServices] IProductoService productoService, ProductoPostDto productoDto) =>
            {
                productoService.AddProducto(productoDto);
                return Results.Ok(productoDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            // PUT: Actualiza un producto existente
            group.MapPut("", ([FromServices] IProductoService productoService, ProductoPutDto productoDto) =>
            {
                productoService.UpdateProducto(productoDto);
                return Results.Ok(productoDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            // DELETE: Elimina un producto por su ID
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