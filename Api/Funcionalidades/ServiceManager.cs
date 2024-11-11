using Api.Funcionalidades.Carritos;
using Api.Funcionalidades.HistorialPrecios;
using Api.Funcionalidades.Usuarios;
using Api.Funcionalidades.Productos;
using Api.Funcionalidades.Categorias;
using Api.Funcionalidades.Roles;
using Api.Funcionalidades.Auth;
using Api.Funcionalidades.ItemCarritos;
namespace Api.Funcionalidades;

public static class ServiceManager
{
    public static IServiceCollection AddServicesManager(this IServiceCollection services)
    {
        services.AddScoped<ICarritoService, CarritoService>();
        services.AddScoped<IAuthService, AuthService>(); 
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IHistorialPrecioServices, HistorialPrecioServices>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IItemCarritoServices, ItemCarritoService>();
        return services;
    }
}
