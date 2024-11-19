using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Api.Funcionalidades.ItemCarritos;

// DTOs para el manejo de items del carrito de compras
namespace Api.Funcionalidades.ItemCarritos;

// DTO base para crear un item en el carrito, hereda de ItemCarritoUpdateDto
public class ItemCarritoDto : ItemCarritoUpdateDto
{
    [Required]
    public Guid? ProductoId { get; set; }  // ID del producto a agregar al carrito
    public Guid CarritoId { get; set; } = Guid.Empty;  // ID del carrito (opcional, si es Empty se crea uno nuevo)
}

// DTO para seleccionar/obtener items del carrito, incluye información adicional
public class ItemCarritoSelectDto : ItemCarritoDto
{
    public bool Entregado { get; set; }    // Indica si el item ya fue entregado
    public Guid Id { get; set; }           // ID único del item en el carrito
    public decimal Subtotal { get; set; }  // Subtotal del item (precio * cantidad)
}

// DTO base para actualizar la cantidad de un item
public class ItemCarritoUpdateDto
{
    [Required]
    public int Cantidad { get; set; }  // Cantidad del producto en el carrito
}
