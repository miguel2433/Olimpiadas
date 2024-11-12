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

            group.MapGet("{id}", ([FromServices] IHistorialPrecioServices historialPrecioService, Guid id) =>
            {
                return Results.Ok(historialPrecioService.GetHistorialPrecio(id));
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapPost("", ([FromServices] IHistorialPrecioServices historialPrecioService, HistorialPrecioDto historialPrecioDto) =>
            {
                historialPrecioService.AddHistorialPrecio(historialPrecioDto);
                return Results.Ok(historialPrecioDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPut("{id}", ([FromServices] IHistorialPrecioServices historialPrecioService, Guid id, HistorialPrecioUpdateDto historialPrecioDto) =>
            {
                historialPrecioService.UpdateHistorialPrecio(id, historialPrecioDto);
                return Results.Ok(historialPrecioDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapDelete("{id}", ([FromServices] IHistorialPrecioServices historialPrecioService, Guid id) =>
            {
                historialPrecioService.DeleteHistorialPrecio(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}