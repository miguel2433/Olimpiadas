using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.HistorialPrecios
{
    public class HistorialPrecioEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/historial-precio")
                .WithTags("Historial de Precios");

            group.MapGet("", ([FromServices] IHistorialPrecioServices historialPrecioService) =>
            {
                return Results.Ok(historialPrecioService.GetHistorialPrecio());
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapPost("", ([FromServices] IHistorialPrecioServices historialPrecioService, HistorialPrecio historialPrecio) =>
            {
                historialPrecioService.AddHistorialPrecio(historialPrecio);
                return Results.Ok(historialPrecio);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPut("{id}", ([FromServices] IHistorialPrecioServices historialPrecioService, int id, HistorialPrecio historialPrecio) =>
            {
                historialPrecioService.UpdateHistorialPrecio(id, historialPrecio);
                return Results.Ok(historialPrecio);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapDelete("{id}", ([FromServices] IHistorialPrecioServices historialPrecioService, int id) =>
            {
                historialPrecioService.DeleteHistorialPrecio(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}