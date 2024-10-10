using Microsoft.EntityFrameworkCore;

namespace Api.Persistencia;

public class AppDbContext : DbContext
{
    public DbSet<Carrito> Carrito { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Pedido> Pedido { get; set; }
    public DbSet<DetallePedido> DetallePedido { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Evento> Evento { get; set; }
    public DbSet<ProductoEvento> ProductoEvento { get; set; }
    public DbSet<Rol> Rol { get; set; }
    public DbSet<UsuarioRol> UsuarioRol { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrito>().ToTable("Carrito");
    }

}