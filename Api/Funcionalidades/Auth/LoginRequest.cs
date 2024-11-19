using System.ComponentModel.DataAnnotations;

namespace Api.Funcionalidades.Auth
{
    // Esta clase define la estructura de datos necesaria para las solicitudes de inicio de sesión
    // Se utiliza para validar y procesar las credenciales del usuario cuando intenta autenticarse
    public class LoginRequest
    {
        // Email del usuario, campo obligatorio para el inicio de sesión
        [Required]
        public string Email { get; set; }

        // Contraseña del usuario, campo obligatorio para el inicio de sesión
        [Required]
        public string Password { get; set; }
    }
}