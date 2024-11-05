using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Dominio;

[Table("HistorialPrecio")]
public class HistorialPrecio
{
    [Key]
    public int Id { get; set; }
    [Required]
    public decimal Precio { get; set; }
    [Required]
    public DateTime FechaCambio { get; set; } = DateTime.UtcNow;
    [Required]
    [ForeignKey("ProductoId")]
    public Guid ProductoId { get; set; }

    public Producto Producto { get; set; }
}
