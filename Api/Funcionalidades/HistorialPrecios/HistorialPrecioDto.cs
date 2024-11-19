using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Funcionalidades.HistorialPrecios
{
    // Este namespace contiene las clases DTO (Data Transfer Object) para el manejo del historial de precios
    public class HistorialPrecioDto : HistorialPrecioUpdateDto
    {
        [Required] // Indica que este campo es obligatorio
        public Guid ProductoId { get; set; } // ID del producto al que pertenece el historial
    }

    // DTO para actualizaciones que solo contiene el precio
    public class HistorialPrecioUpdateDto
    {
        [Required] // Indica que este campo es obligatorio
        public decimal Precio { get; set; } // Precio del producto
    }

    // DTO para obtener información completa del historial, incluyendo ID y fecha
    public class HistorialPrecioGetDto : HistorialPrecioDto
    {
        public Guid Id { get; set; } // ID único del registro del historial
        public DateTime FechaCambio { get; set; } // Fecha en que se realizó el cambio de precio
    }
}
