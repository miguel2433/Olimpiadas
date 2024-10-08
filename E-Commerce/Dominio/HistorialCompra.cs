using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Dominio
{
    public class HistorialCompra
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Fecha { get; set; }
        public List<Producto> Productos { get; set; }
        public decimal Total { get; set; }
    }
}