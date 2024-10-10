using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Dominio;
using System.ComponentModel.DataAnnotations;

public class Categoria
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }
    [StringLength(255)]
    public string? Descripcion { get; set; }
<<<<<<< HEAD
    public List<Producto> Productos { get; set; }
}
=======
    public List<Producto> Productos { get; set; } = new List<Producto>();
    public bool Eliminado { get; set; } = false;
} 
>>>>>>> 3b7e3246b744081bbe1a3ca86ff5a4695878aac8
