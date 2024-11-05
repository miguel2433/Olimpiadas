using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Dominio;
[Table("Producto")]
public class Producto
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }
    [StringLength(255)]
    public string? Descripcion { get; set; }
    [Required]
    public int Stock { get; set; }  
    [StringLength(255)]
    public string? UrlImagen { get; set; }
    [ForeignKey("VendedorId")]
    public Guid VendedorId { get; set; }
    [Required]
    public Usuario Vendedor { get; set; }
    public List<Categoria> Categorias { get; set; } = new List<Categoria>();
    public List<HistorialPrecio> HistorialPrecios { get; set; } = new List<HistorialPrecio>();
    public bool Eliminado { get; set; } = false;
}