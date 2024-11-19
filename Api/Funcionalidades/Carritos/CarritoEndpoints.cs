using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;
namespace Api.Funcionalidades.Carritos;

// Esta clase define todos los endpoints relacionados con la funcionalidad de carritos
// Implementa ICarterModule para la configuración de rutas en la API
public class CarritoEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Configura un grupo de rutas bajo /api/carrito
        var group = app.MapGroup("/api/carrito")
            .WithTags("Carritos");

        // Endpoint para obtener los carritos del usuario actual
        group.MapGet("", ([FromServices] ICarritoService carritoService) =>
        {
            return Results.Ok(carritoService.GetCarritoPorUsuario());
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para eliminar un carrito específico
        group.MapDelete("{id}", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.DeleteCarrito(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para buscar carritos que contengan un producto específico
        group.MapGet("/producto/{id}", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            var carrito = carritoService.BuscarCarritoPorProducto(id);
            if (carrito != null)
            {
                return Results.Ok(carrito);
            }
            return Results.NotFound();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para marcar un carrito como entregado
        group.MapPut("/{id}/entregado", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.MarcarComoEntregado(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para calcular el total de un carrito
        group.MapGet("/{id}/total", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            var total = carritoService.CalcularTotal(id);
            return Results.Ok(total);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para marcar un carrito como eliminado
        group.MapPut("/{id}/eliminado", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.MarcarComoEliminado(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // Endpoint para procesar el pago de un carrito
        group.MapPut("{id}/pagar", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.PagarCarrito(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }

}
