using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Dominio;

public class Producto
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Nombre { get; set; }
    public string? Descripcion { get; set; }
    [Required]
    public decimal Precio { get; set; }
    [Required]
    public int Stock { get; set; }  
    public string? UrlImagen { get; set; }
    [Required]
    public Categoria Categoria { get; set; }
    public bool Eliminado { get; set; } = false;
}