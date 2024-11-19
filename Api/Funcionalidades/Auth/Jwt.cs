// Esta clase representa la configuración necesaria para generar y validar tokens JWT (JSON Web Tokens)
// Se utiliza para la autenticación y autorización de usuarios en la API
namespace Api.Funcionalidades.Auth
{
    public class Jwt
    {
        // Clave secreta utilizada para firmar el token
        public string Key { get; set; }

        // Identifica quién emitió el token (por ejemplo, el nombre de tu aplicación)
        public string Issuer { get; set; }

        // Identifica para quién está destinado el token (por ejemplo, tu API)
        public string Audience { get; set; }
    }
}