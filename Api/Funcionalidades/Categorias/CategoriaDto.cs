using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Funcionalidades.Categorias;

public class CategoriaDto : CategoriaUpdateDto
{
    public Guid Id { get; set; }
    public List<Guid> Productos { get; set; } = new List<Guid>();
    public bool Eliminado { get; set; } = false;
}

public class CategoriaUpdateDto
{
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }
    
    [StringLength(255)]
    public string? Descripcion { get; set; }
}