using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Funcionalidades.HistorialPrecios
{
    public class HistorialPrecioDto : HistorialPrecioUpdateDto
    {
        [Required]
        public Guid ProductoId { get; set; }

    }

    public class HistorialPrecioUpdateDto
    {
        [Required]
        public decimal Precio { get; set; }
    }

    public class HistorialPrecioGetDto : HistorialPrecioDto
    {
        public Guid Id { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
