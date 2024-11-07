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

            group.MapGet("", ([FromServices] IItemCarritoServices itemCarritoService) =>
            {
                return Results.Ok(itemCarritoService.GetItemCarrito());
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapPost("", ([FromServices] IItemCarritoServices itemCarritoService, ItemCarrito itemCarrito) =>
            {
                itemCarritoService.AddItemCarrito(itemCarrito);
                return Results.Ok(itemCarrito);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
            
            group.MapPut("{id}", ([FromServices] IItemCarritoServices itemCarritoService, ItemCarrito itemCarrito) =>
            {
                itemCarritoService.UpdateItemCarrito(itemCarrito);
                return Results.Ok(itemCarrito);
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