using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.Carritos;

public class CarritoEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/carrito", async ([FromServices] ICarritoService carritoService) =>
        {
            return Results.Ok(carritoService.GetCarrito());
        });
    }
    
}