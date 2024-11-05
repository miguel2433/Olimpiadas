// Importaciones necesarias para el funcionamiento del endpoint de autenticación
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Api.Funcionalidades.Auth;
using Microsoft.AspNetCore.Identity.Data;

namespace Api.Funcionalidades.Auth
{
    // Clase que define los endpoints relacionados con la autenticación
    public class AuthEndpoints : ICarterModule
    {
        // Método requerido por ICarterModule para configurar las rutas
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Configura el endpoint POST para el login
            app.MapPost("/api/auth/login", async ([FromServices] IAuthService authService, LoginRequest loginRequest) =>
            {
                // Valida que se hayan proporcionado email y contraseña
                if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return Results.BadRequest("El email y la contraseña son obligatorios.");
                }

                // Intenta realizar el login y obtener el token
                var token = await authService.Login(loginRequest);
                // Si el token es null, significa que las credenciales son inválidas
                if (token == null)
                {
                    return Results.Unauthorized();
                }
                // Retorna el token en caso de éxito
                return Results.Ok(new { Token = token });
            })
            .WithName("Login") // Nombre de la ruta para referencia
            .WithTags("Autenticación") // Etiqueta para agrupar en Swagger
            .Produces(StatusCodes.Status200OK) // Documenta las posibles respuestas HTTP
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}
