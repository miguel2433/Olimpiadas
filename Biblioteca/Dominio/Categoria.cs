using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Dominio;

public class Categoria
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }
    [StringLength(255)]
    public string? Descripcion { get; set; }
    public List<Producto> Productos { get; set; } = new List<Producto>();
    public bool Eliminado { get; set; } = false;
} 