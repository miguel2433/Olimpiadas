using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Dominio;

public class Carrito
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
<<<<<<< HEAD
    [ForeignKey("UsuarioId")]
    public Guid UsuarioId { get; set; }
=======
<<<<<<< HEAD
    public  List<Producto> Productos { get; set; }
=======
    public List<Producto> Productos { get; set; } = new List<Producto>();
>>>>>>> 3b7e3246b744081bbe1a3ca86ff5a4695878aac8
    public decimal Total { get; set; }
>>>>>>> 63d2b9b1b7d56b1c7a05121f2b92940b024d2d34
    [Required]
    public Usuario Usuario { get; set; }
    public List<Producto> Productos { get; set; } = new List<Producto>();
    public decimal Total { get; set; }
    public bool Eliminado { get; set; } = false;
}
