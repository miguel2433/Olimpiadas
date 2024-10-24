using System.ComponentModel.DataAnnotations;

namespace Api.Funcionalidades.Auth
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}