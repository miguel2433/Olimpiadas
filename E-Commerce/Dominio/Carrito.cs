using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Dominio;

public class Carrito
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
<<<<<<< HEAD
    public  List<Producto> Productos { get; set; }
=======
    public List<Producto> Productos { get; set; } = new List<Producto>();
>>>>>>> 3b7e3246b744081bbe1a3ca86ff5a4695878aac8
    public decimal Total { get; set; }
    [Required]
    public Usuario Usuario { get; set; }
    public bool Eliminado { get; set; } = false;
}
