using Api.Funcionalidades.Carritos;
using Api.Funcionalidades.HistorialCompras;
using Api.Funcionalidades.Usuarios;
using Api.Funcionalidades.Productos;
using Api.Funcionalidades.Categorias;
using Api.Funcionalidades.Roles;
using Api.Funcionalidades.Auth;

namespace Api.Funcionalidades;

public static class ServiceManager
{
    public static IServiceCollection AddServicesManager(this IServiceCollection services)
    {
        services.AddScoped<ICarritoService, CarritoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IHistorialCompraServices, HistorialCompraServices>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
