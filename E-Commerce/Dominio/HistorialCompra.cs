namespace E_Commerce.Dominio;

public class HistorialCompra
{
<<<<<<< HEAD
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Fecha { get; set; }
    public List<Producto> Productos { get; set; }
    public decimal Total { get; set; }
=======
    public class HistorialCompra
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required]
        public Carrito Carrito { get; set; }
        public bool Eliminado { get; set; } = false;
    }
>>>>>>> 3b7e3246b744081bbe1a3ca86ff5a4695878aac8
}