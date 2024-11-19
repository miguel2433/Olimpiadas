using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.ItemCarritos
{
    /// <summary>
    /// Clase que define los endpoints de la API para el manejo de items del carrito
    /// </summary>
    public class ItemCarritoEndpoints : ICarterModule
    {
        /// <summary>
        /// Configura las rutas y endpoints disponibles
        /// </summary>
        /// <param name="app">Constructor de rutas de la aplicación</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Crea un grupo de rutas con el prefijo /api/item-carrito
            var group = app.MapGroup("/api/item-carrito")
                .WithTags("Item Carrito");

            // GET: Obtiene los items de un carrito específico
            group.MapGet("", ([FromServices] IItemCarritoServices itemCarritoService, Guid carritoId) =>
            {
                return Results.Ok(itemCarritoService.GetItemCarritoPorCarritoId(carritoId));
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            // POST: Agrega un nuevo item al carrito
            group.MapPost("", ([FromServices] IItemCarritoServices itemCarritoService, ItemCarritoDto itemCarritoDto) =>
            {
                itemCarritoService.AddItemCarrito(itemCarritoDto);
                return Results.Ok(itemCarritoDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            // PUT: Actualiza la cantidad de un item existente
            group.MapPut("{id}", ([FromServices] IItemCarritoServices itemCarritoService, ItemCarritoUpdateDto itemCarritoDto, Guid id) =>
            {
                itemCarritoService.UpdateItemCarrito(itemCarritoDto, id);
                return Results.Ok(itemCarritoDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            // DELETE: Elimina un item del carrito
            group.MapDelete("{id}", ([FromServices] IItemCarritoServices itemCarritoService, Guid id) =>
            {
                itemCarritoService.DeleteItemCarrito(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            // PUT: Marca un item como entregado
            group.MapPut("{id}/entregado", ([FromServices] IItemCarritoServices itemCarritoService, Guid id) =>
            {
                itemCarritoService.MarcarComoEntregadoItem(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            // PUT: Rechaza un item y restaura el stock
            group.MapPut("{id}/rechazar", ([FromServices] IItemCarritoServices itemCarritoService, Guid id) =>
            {
                itemCarritoService.RechazarItem(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}