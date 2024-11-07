using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Dominio;
[Table("Carrito")]
public class Carrito
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [ForeignKey("UsuarioId")]
    public Guid UsuarioId { get; set; }
    [Required]
    public Usuario Usuario { get; set; }
    public List<ItemCarrito> Items { get; set; } = new List<ItemCarrito>();
    public decimal Total { get; set; }
    public bool Entregado { get; set; } = false;
    public bool Pagado { get; set; } = false;
    public bool Eliminado { get; set; } = false;
    public DateTime Fecha { get; set; } = DateTime.Now;
}
