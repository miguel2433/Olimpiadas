## Configuración del `appsettings.json`

Asegúrate de configurar correctamente tu archivo `appsettings.json` con la cadena de conexión y otros parámetros importantes. Aquí tienes un ejemplo:

  ```json
{
  "AllowedHosts": "*",
  "ConnectionStrings": 
  {
    "DefaultConnection": "Server=localhost;Database=5to_OrganizacionEventosEscolares;User=tuUsuario;Password=tuPassword;"
  },
  "Logging": 
  {
    "LogLevel": 
    {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

