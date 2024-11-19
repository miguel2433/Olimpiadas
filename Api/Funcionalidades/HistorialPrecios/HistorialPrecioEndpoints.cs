using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.HistorialPrecios
{
    // Clase que define los endpoints de la API para el historial de precios
    public class HistorialPrecioEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Crea un grupo de rutas con el prefijo /api/historial-precio
            var group = app.MapGroup("/api/historial-precio")
                .WithTags("Historial de Precios"); // Agrega un tag para la documentación

            // GET: Obtiene el historial de precios de un producto específico
            group.MapGet("{id}", ([FromServices] IHistorialPrecioServices historialPrecioService, Guid id) =>
            {
                return Results.Ok(historialPrecioService.GetHistorialPrecio(id));
            })
            .Produces(StatusCodes.Status200OK) // Documenta las respuestas posibles
            .Produces(StatusCodes.Status401Unauthorized);

            // POST: Crea un nuevo registro en el historial de precios
            group.MapPost("", ([FromServices] IHistorialPrecioServices historialPrecioService, HistorialPrecioDto historialPrecioDto) =>
            {
                historialPrecioService.AddHistorialPrecio(historialPrecioDto);
                return Results.Ok(historialPrecioDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            // PUT: Actualiza un registro existente del historial
            group.MapPut("{id}", ([FromServices] IHistorialPrecioServices historialPrecioService, Guid id, HistorialPrecioUpdateDto historialPrecioDto) =>
            {
                historialPrecioService.UpdateHistorialPrecio(id, historialPrecioDto);
                return Results.Ok(historialPrecioDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            // DELETE: Elimina un registro del historial
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