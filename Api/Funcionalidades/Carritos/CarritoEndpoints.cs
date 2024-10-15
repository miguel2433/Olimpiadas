using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;
namespace Api.Funcionalidades.Carritos;

public class CarritoEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/carrito", ([FromServices] ICarritoService carritoService) =>
        {
            return Results.Ok(carritoService.GetCarrito());
        });
        app.MapPost("/api/carrito", ([FromServices] ICarritoService carritoService, Carrito carrito) =>
        {
            carritoService.AddCarrito(carrito);
            return Results.Ok(carrito);
        }); 
        app.MapPut("/api/carrito/{id}", ([FromServices] ICarritoService carritoService, Guid id, Carrito carrito) =>
        {
            carritoService.UpdateCarrito(id, carrito);
            return Results.Ok(carrito);
        });
        app.MapDelete("/api/carrito/{id}", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.DeleteCarrito(id);
            return Results.Ok();
        });
        app.MapGet("/api/carrito/producto/{id}", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            var carrito = carritoService.BuscarCarritoPorProducto(id);
            if (carrito != null)
            {
                return Results.Ok(carrito);
            }
            return Results.NotFound();
        });
        app.MapPut("/api/carrito/{id}/entregado", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.MarcarComoEntregado(id);
            return Results.Ok();
        });
        app.MapGet("/api/carrito/{id}/total", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            var total = carritoService.CalcularTotal(id);
            return Results.Ok(total);
        });
        app.MapPut("/api/carrito/{id}/eliminado", ([FromServices] ICarritoService carritoService, Guid id) =>
        {
            carritoService.MarcarComoEliminado(id);
            return Results.Ok();
        });
    }
}
