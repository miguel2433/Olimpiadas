using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Dominio;

public class Categoria
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public List<Producto> Productos { get; set; } = new List<Producto>();
    public bool Eliminado { get; set; } = false;
} 