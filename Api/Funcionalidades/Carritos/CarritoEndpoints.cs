using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;
namespace Api.Funcionalidades.Carritos;

public class CarritoEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/carrito")
            .WithTags("Carritos");

        group.MapGet("", ([FromServices] ICarritoService carritoService) =>
        {
            return Results.Ok(carritoService.GetCarritoPorUsuario());
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapDelete("{id}", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.DeleteCarrito(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

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

        group.MapPut("/{id}/entregado", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.MarcarComoEntregado(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/{id}/total", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            var total = carritoService.CalcularTotal(id);
            return Results.Ok(total);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/{id}/eliminado", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.MarcarComoEliminado(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("{id}/pagar", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.PagarCarrito(id);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);
    }

}
