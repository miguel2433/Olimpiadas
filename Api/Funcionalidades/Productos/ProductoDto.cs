using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Api.Funcionalidades.Productos
{
    public class ProductoPostDto
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(255)]
        public string? Descripcion { get; set; }
        [Required]
        public int Stock { get; set; }
        [StringLength(255)]
        public string? UrlImagen { get; set; }
        [Required]
        public List<Guid> CategoriaIds { get; set; }
        [Required]
        public decimal Precio { get; set; }
    }

    public class ProductoPutDto : ProductoPostDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}