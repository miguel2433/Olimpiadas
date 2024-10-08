using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Dominio;

public class Carrito
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<Producto> Productos { get; set; }
    public decimal Total { get; set; }
    public Usuario Usuario { get; set; }
}