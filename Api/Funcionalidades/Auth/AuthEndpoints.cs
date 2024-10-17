using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Api.Funcionalidades.Auth;
namespace Api.Funcionalidades.Auth
{
    public class AuthEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/login", async ([FromServices] IAuthService authService, LoginRequest loginRequest) =>
            {
                if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return Results.BadRequest("El email y la contraseña son obligatorios.");
                }

                var token = await authService.Login(loginRequest.Email, loginRequest.Password);
                if (token == null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(new { Token = token });
            })
            .WithName("Login")
            .WithTags("Autenticación")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}
