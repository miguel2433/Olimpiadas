using Api.Funcionalidades.Carritos;

namespace Api.Funcionalidades;

public static class ServiceManager
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICarritoService, CarritoService>();
        return services;
    }
}