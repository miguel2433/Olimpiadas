using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Dominio;
[Table("Usuario")]
public class Usuario
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }
    [Required]
    [StringLength(50)]
    public string NombreUsuario { get; set; }
    [Required]
    [StringLength(50)]
    public string Apellido { get; set; }
    [Required]
    [StringLength(50)]
    public string Email { get; set; }
    [Required]
    [StringLength(50)]
    public string Password { get; set; }
    [StringLength(50)]
    public string Telefono { get; set; } = string.Empty;
    public List<Carrito>? Carrito { get; set; }
    public bool Eliminado { get; set; } = false;
}
