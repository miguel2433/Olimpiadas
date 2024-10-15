using Microsoft.EntityFrameworkCore;
using Biblioteca.Dominio;

namespace Api.Persistencia;

public class AppDbContext : DbContext
{
    public DbSet<Carrito> Carrito { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<HistorialCompra> HistorialCompra { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrito>().ToTable("Carrito");
        modelBuilder.Entity<Producto>().ToTable("Producto");
        modelBuilder.Entity<Usuario>().ToTable("Usuario");
        
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categoria");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Descripcion).HasMaxLength(500);
        });
        
        modelBuilder.Entity<HistorialCompra>(entity =>
        {
            entity.ToTable("HistorialCompra");
            entity.HasKey(h => h.Id);
            entity.Property(h => h.FechaCompra).IsRequired();
            entity.Property(h => h.Eliminado).IsRequired();
            entity.HasOne(h => h.Carrito).WithMany().HasForeignKey(h => h.CarritoId);
        });

    }

}