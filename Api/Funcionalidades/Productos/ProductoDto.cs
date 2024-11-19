using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
//
namespace Api.Funcionalidades.Productos
{
    // DTOs para el manejo de productos
    public class ProductoPostDto
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }     // Nombre del producto
        [StringLength(255)]
        public string? Descripcion { get; set; }// Descripción opcional
        [Required]
        public int Stock { get; set; }         // Cantidad disponible
        [StringLength(255)]
        public string? UrlImagen { get; set; } // URL de la imagen del producto
        public List<Guid>? CategoriaIds { get; set; } // Categorías asociadas
        [Required]
        public decimal Precio { get; set; }    // Precio del producto
    }

    // DTO para actualizar productos, hereda de ProductoPostDto
    public class ProductoPutDto : ProductoPostDto
    {
        [Required]
        public Guid Id { get; set; }  // ID del producto a actualizar
    }
}