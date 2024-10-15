using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.HistorialCompras
{
    public class HistorialCompraEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/historial-compra")
                .WithTags("Historial de Compras");

            group.MapGet("", ([FromServices] IHistorialCompraServices historialCompraService) =>
            {
                return Results.Ok(historialCompraService.GetHistorialCompra());
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapPost("", ([FromServices] IHistorialCompraServices historialCompraService, HistorialCompra historialCompra) =>
            {
                historialCompraService.AddHistorialCompra(historialCompra);
                return Results.Ok(historialCompra);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPut("{id}", ([FromServices] IHistorialCompraServices historialCompraService, Guid id, HistorialCompra historialCompra) =>
            {
                historialCompraService.UpdateHistorialCompra(id, historialCompra);
                return Results.Ok(historialCompra);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapDelete("{id}", ([FromServices] IHistorialCompraServices historialCompraService, Guid id) =>
            {
                historialCompraService.DeleteHistorialCompra(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}