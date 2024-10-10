using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Dominio;

public class Carrito
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<Producto> Productos { get; set; } = new List<Producto>();
    public decimal Total { get; set; }
    [Required]
    public Usuario Usuario { get; set; }
    public bool Eliminado { get; set; } = false;
}
