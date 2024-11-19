using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Funcionalidades.Categorias;

// Esta clase representa el DTO principal para las categorías, hereda de CategoriaUpdateDto
// Se utiliza para la transferencia completa de datos de categorías incluyendo sus relaciones
public class CategoriaDto : CategoriaUpdateDto
{
    // Identificador único de la categoría
    public Guid Id { get; set; }

    // Lista de identificadores de productos asociados a esta categoría
    public List<Guid> Productos { get; set; } = new List<Guid>();

    // Indica si la categoría ha sido marcada como eliminada (soft delete)
    public bool Eliminado { get; set; } = false;
}

// Esta clase define los campos actualizables de una categoría
// Se utiliza para operaciones de creación y actualización
public class CategoriaUpdateDto
{
    // Nombre de la categoría, requerido y con longitud máxima de 50 caracteres
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }
    
    // Descripción opcional de la categoría, con longitud máxima de 255 caracteres
    [StringLength(255)]
    public string? Descripcion { get; set; }
}