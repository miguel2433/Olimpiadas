namespace Api.Persistencia
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Rol.Any())
            {
                context.Rol.AddRange(
                    new Rol { Nombre = "Administrador" },
                    new Rol { Nombre = "Vendedor" },
                    new Rol { Nombre = "Usuario" }
                );
                context.SaveChanges();
            }
            if (!context.Usuario.Any())
            {
                context.Usuario.AddRange(
                    new Usuario { Nombre = "Administrador", Email = "admin@gmail.com", Password = "123456", RolId = context.Rol.FirstOrDefault(r => r.Nombre == "Administrador")?.Id },
                    new Usuario { Nombre = "Vendedor", Email = "vendedor@gmail.com", Password = "123456", RolId = context.Rol.FirstOrDefault(r => r.Nombre == "Vendedor")?.Id },
                    new Usuario { Nombre = "Usuario", Email = "usuario@gmail.com", Password = "123456", RolId = context.Rol.FirstOrDefault(r => r.Nombre == "Usuario")?.Id }
                );
                context.SaveChanges();
            }
        }
    }
}