using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Biblioteca.Dominio;

[Table("HistorialCompra")]
public class HistorialCompra
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime FechaCompra { get; set; } = DateTime.Now;
    [Required]
    public Carrito Carrito { get; set; }
    public bool Eliminado { get; set; } = false;
    [ForeignKey("CarritoId")]
    public Guid CarritoId { get; set; }
}