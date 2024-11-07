using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Biblioteca.Dominio
{
    [Table("ItemCarrito")]
    public class ItemCarrito
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Producto Producto { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Subtotal { get; set; }
        [Required]
        [ForeignKey("CarritoId")]
        public Carrito Carrito { get; set; }
        [Required]
        public bool Entregado { get; set; } = false;
    }
}
