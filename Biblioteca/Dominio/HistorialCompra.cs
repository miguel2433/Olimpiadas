using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Dominio
{
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
}