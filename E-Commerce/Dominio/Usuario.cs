using System.ComponentModel.DataAnnotations;
namespace E_Commerce.Dominio;

public class Usuario
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public List<Carrito> Carrito { get; set; }
}
