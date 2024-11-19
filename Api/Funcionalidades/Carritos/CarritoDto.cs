using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Funcionalidades.Carritos;

// Esta clase DTO (Data Transfer Object) representa la estructura de datos de un carrito
// Se utiliza para transferir información del carrito entre las diferentes capas de la aplicación
public class CarritoDto
{
    // Identificador único del carrito
    public Guid Id { get; set; }

    // Identificador del usuario propietario del carrito
    public Guid UsuarioId { get; set; }

    // Indica si el carrito ha sido entregado
    public bool Entregado { get; set; } = false;

    // Indica si el carrito ha sido pagado
    public bool Pagado { get; set; } = false;

    // Indica si el carrito ha sido marcado como eliminado
    public bool Eliminado { get; set; } = false;

    // Lista de identificadores de los items en el carrito
    public List<Guid> Items { get; set; } = new List<Guid>();
}
