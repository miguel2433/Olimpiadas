using Microsoft.EntityFrameworkCore;
using Biblioteca.Dominio;

namespace Api.Persistencia;

public class AppDbContext : DbContext
{
    public DbSet<Carrito> Carrito { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<HistorialPrecio> HistorialPrecio { get; set; }
    public DbSet<Rol> Rol { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrito>().ToTable("Carrito");
        modelBuilder.Entity<Producto>().ToTable("Producto");
        modelBuilder.Entity<Usuario>().ToTable("Usuario");
        modelBuilder.Entity<Rol>().ToTable("Rol");
        modelBuilder.Entity<HistorialPrecio>().ToTable("HistorialPrecio");
        modelBuilder.Entity<Categoria>().ToTable("Categoria");
    }

}