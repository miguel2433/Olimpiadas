using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Api.Funcionalidades.Usuarios;

/// <summary>
/// Clase DTO que representa los datos de transferencia para un usuario
/// </summary>
public class UsuarioDto
{
    /// <summary>
    /// Nombre del usuario
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }

    /// <summary>
    /// Nombre de usuario para el sistema
    /// </summary>
    [Required]
    [StringLength(50)]
    public string NombreUsuario { get; set; }

    [Required]
    [StringLength(50)]
    public string Apellido { get; set; }
    [Required]
    [StringLength(50)]
    public string Email { get; set; }
    [Required]
    [StringLength(50)]
    public string Password { get; set; }
    [StringLength(50)]
    public string Telefono { get; set; } = string.Empty;

}
