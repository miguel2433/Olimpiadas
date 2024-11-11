using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Api.Funcionalidades.ItemCarritos;

public class ItemCarritoDto
{
    [Required]
    public Guid? ProductoId { get; set; }
    [Required]
    public int Cantidad { get; set; }
    public Guid CarritoId { get; set; } = Guid.Empty;
}

public class ItemCarritoSelectDto : ItemCarritoDto
{
    public bool Entregado { get; set; }
    public Guid Id { get; set; }
    public decimal Subtotal { get; set; }
}