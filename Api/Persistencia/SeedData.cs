using Biblioteca.Dominio;

namespace Api.Persistencia
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            if (!context.Rol.Any())
            {
                context.Rol.AddRange(
                    new Rol { Nombre = "Administrador", Descripcion = "TodoPoderoso"},
                    new Rol { Nombre = "Vendedor", Descripcion = "Vende"},
                    new Rol { Nombre = "Usuario", Descripcion = "Cliente" }
                );
                context.SaveChanges();
            }
            if (!context.Usuario.Any())
            {
                context.Usuario.AddRange(
                    new Usuario { NombreUsuario = "Administrador", Nombre = "Administrador", Apellido = "nada", Email = "admin@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123456"), Rol = context.Rol.FirstOrDefault(r => r.Nombre == "Administrador") },
                    new Usuario { Nombre = "Vendedor", NombreUsuario = "Vendedor", Apellido = "nada", Email = "vendedor@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123456"), Rol = context.Rol.FirstOrDefault(r => r.Nombre == "Vendedor") },
                    new Usuario { Nombre = "Usuario", Apellido = "nada", NombreUsuario = "Usuario", Email = "usuario@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123456"), Rol = context.Rol.FirstOrDefault(r => r.Nombre == "Usuario") }
                );
                context.SaveChanges();
            }
        }
    }
}