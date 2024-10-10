using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Dominio;

public class Carrito
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [ForeignKey("UsuarioId")]
    public Guid UsuarioId { get; set; }
    [Required]
    public Usuario Usuario { get; set; }
    public List<Producto> Productos { get; set; } = new List<Producto>();
    public decimal Total { get; set; }
    public bool Entregado { get; set; } = false;
    public bool Eliminado { get; set; } = false;
}
