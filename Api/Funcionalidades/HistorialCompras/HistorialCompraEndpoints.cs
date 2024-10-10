using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Funcionalidades.HistorialCompras;
using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.HistorialCompras
{
    public class HistorialCompraEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/historial-compra", ([FromServices] IHistorialCompraServices historialCompraService) =>
            {
                return Results.Ok(historialCompraService.GetHistorialCompra());
            });
            app.MapPost("/api/historial-compra", ([FromServices] IHistorialCompraServices historialCompraService, HistorialCompra historialCompra) =>
            {
                historialCompraService.AddHistorialCompra(historialCompra);
                return Results.Ok(historialCompra);
            }); 
            app.MapPut("/api/historial-compra/{id}", ([FromServices] IHistorialCompraServices historialCompraService, int id, HistorialCompra historialCompra) =>
            {
                historialCompraService.UpdateHistorialCompra(id, historialCompra);
                return Results.Ok(historialCompra);
            });
            app.MapDelete("/api/historial-compra/{id}", ([FromServices] IHistorialCompraServices historialCompraService, int id) =>
            {
                historialCompraService.DeleteHistorialCompra(id);
                return Results.Ok();
            });
        }
    }
}