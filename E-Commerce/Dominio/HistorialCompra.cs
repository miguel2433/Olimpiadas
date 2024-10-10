using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Dominio
{
    public class HistorialCompra
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required]
        public Carrito Carrito { get; set; }
        public bool Eliminado { get; set; } = false;
    }
}