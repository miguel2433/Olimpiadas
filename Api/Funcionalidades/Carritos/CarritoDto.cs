using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Funcionalidades.Carritos;

public class CarritoDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public bool Entregado { get; set; } = false;
    public bool Pagado { get; set; } = false;
    public bool Eliminado { get; set; } = false;
    public List<Guid> Items { get; set; } = new List<Guid>();
}
