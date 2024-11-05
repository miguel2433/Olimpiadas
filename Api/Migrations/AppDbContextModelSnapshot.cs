﻿// <auto-generated />
using System;
using Api.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Biblioteca.Dominio.Carrito", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Entregado")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Pagado")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Carrito", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Dominio.Categoria", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categoria", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Dominio.HistorialPrecio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaCambio")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("ProductoId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ProductoId");

                    b.ToTable("HistorialPrecio", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Dominio.Producto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CarritoId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<string>("UrlImagen")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("VendedorId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CarritoId");

                    b.HasIndex("VendedorId");

                    b.ToTable("Producto", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Dominio.Rol", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Rol", (string)null);
                });

            modelBuilder.Entity("Biblioteca.Dominio.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("Eliminado")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("RolId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("CategoriaProducto", b =>
                {
                    b.Property<Guid>("CategoriasId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ProductosId")
                        .HasColumnType("char(36)");

                    b.HasKey("CategoriasId", "ProductosId");

                    b.HasIndex("ProductosId");

                    b.ToTable("CategoriaProducto");
                });

            modelBuilder.Entity("Biblioteca.Dominio.Carrito", b =>
                {
                    b.HasOne("Biblioteca.Dominio.Usuario", "Usuario")
                        .WithMany("Carrito")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Biblioteca.Dominio.HistorialPrecio", b =>
                {
                    b.HasOne("Biblioteca.Dominio.Producto", "Producto")
                        .WithMany("HistorialPrecios")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("Biblioteca.Dominio.Producto", b =>
                {
                    b.HasOne("Biblioteca.Dominio.Carrito", null)
                        .WithMany("Productos")
                        .HasForeignKey("CarritoId");

                    b.HasOne("Biblioteca.Dominio.Usuario", "Vendedor")
                        .WithMany()
                        .HasForeignKey("VendedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vendedor");
                });

            modelBuilder.Entity("Biblioteca.Dominio.Usuario", b =>
                {
                    b.HasOne("Biblioteca.Dominio.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("CategoriaProducto", b =>
                {
                    b.HasOne("Biblioteca.Dominio.Categoria", null)
                        .WithMany()
                        .HasForeignKey("CategoriasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca.Dominio.Producto", null)
                        .WithMany()
                        .HasForeignKey("ProductosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Biblioteca.Dominio.Carrito", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("Biblioteca.Dominio.Producto", b =>
                {
                    b.Navigation("HistorialPrecios");
                });

            modelBuilder.Entity("Biblioteca.Dominio.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Biblioteca.Dominio.Usuario", b =>
                {
                    b.Navigation("Carrito");
                });
#pragma warning restore 612, 618
        }
    }
}
