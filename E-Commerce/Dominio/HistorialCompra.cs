<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
=======
namespace E_Commerce.Dominio;
>>>>>>> 63d2b9b1b7d56b1c7a05121f2b92940b024d2d34

public class HistorialCompra
{
<<<<<<< HEAD
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Fecha { get; set; }
    public List<Producto> Productos { get; set; }
    public decimal Total { get; set; }
=======
    public class HistorialCompra
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required]
        public Carrito Carrito { get; set; }
        public bool Eliminado { get; set; } = false;
        [ForeignKey("CarritoId")]
        public Guid CarritoId { get; set; }
    }
>>>>>>> 3b7e3246b744081bbe1a3ca86ff5a4695878aac8
}