using Biblioteca.Dominio;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Api.Funcionalidades.ItemCarritos
{
    public class ItemCarritoEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/item-carrito")
                .WithTags("Item Carrito");

            group.MapGet("", ([FromServices] IItemCarritoServices itemCarritoService, Guid carritoId) =>
            {
                return Results.Ok(itemCarritoService.GetItemCarritoPorCarritoId(carritoId));
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapPost("", ([FromServices] IItemCarritoServices itemCarritoService, ItemCarritoDto itemCarritoDto) =>
            {
                itemCarritoService.AddItemCarrito(itemCarritoDto);
                return Results.Ok(itemCarritoDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPut("{id}", ([FromServices] IItemCarritoServices itemCarritoService, ItemCarritoUpdateDto itemCarritoDto, Guid id) =>
            {
                itemCarritoService.UpdateItemCarrito(itemCarritoDto, id);
                return Results.Ok(itemCarritoDto);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapDelete("{id}", ([FromServices] IItemCarritoServices itemCarritoService, Guid id) =>
            {
                itemCarritoService.DeleteItemCarrito(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapPut("{id}/entregado", ([FromServices] IItemCarritoServices itemCarritoService, Guid id) =>
            {
                itemCarritoService.MarcarComoEntregadoItem(id);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

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