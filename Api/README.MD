# Carpeta @Api

## Descripción

La carpeta @Api contiene la implementación del backend de nuestra aplicación. Utiliza ASP.NET Core y sigue una arquitectura basada en endpoints para manejar las solicitudes HTTP.

## Estructura

La carpeta @Api está organizada de la siguiente manera:

- `Funcionalidades`: Contiene la lógica de negocio y los endpoints para cada entidad.
- `Persistencia`: Maneja la conexión y operaciones con la base de datos.
- `Migrations`: Contiene las migraciones de la base de datos.
- `Program.cs`: Punto de entrada de la aplicación y configuración inicial.

## Características principales

1. **Endpoints RESTful**: Implementa operaciones CRUD para las siguientes entidades:
   - Carrito
   - Categoría
   - Historial de Compras
   - Producto
   - Usuario

2. **Base de datos**: Utiliza Entity Framework Core con MySQL como proveedor de base de datos.

3. **Inyección de dependencias**: Configura los servicios necesarios para cada entidad.

4. **Swagger**: Incluye documentación de la API mediante Swagger UI.

## Configuración

La configuración de la base de datos y otros parámetros se realiza en el archivo `appsettings.json`. Asegúrate de configurar correctamente la cadena de conexión:


  ```json
{
  "AllowedHosts": "*",
  "ConnectionStrings": 
  {
    "DefaultConnection": "Server=localhost;Database=5to_Olimpiadas;User=tuUsuario;Password=tuPassword;"
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

````

## Ejecución

Para ejecutar la API:

1. Asegúrate de tener .NET Core SDK instalado.
2. Navega hasta la carpeta @Api en la terminal.
3. Ejecuta `dotnet run`.

La API estará disponible en `https://localhost:5001` por defecto.

## Endpoints principales

- GET `/api/{entidad}`: Obtiene todos los registros de la entidad.
- POST `/api/{entidad}`: Crea un nuevo registro de la entidad.
- PUT `/api/{entidad}/{id}`: Actualiza un registro existente de la entidad.
- DELETE `/api/{entidad}/{id}`: Elimina un registro de la entidad.

Reemplaza `{entidad}` con `carrito`, `categoria`, `historial-compra`, `producto` o `usuario` según corresponda.

## Notas adicionales

- La API utiliza Carter para simplificar la definición de endpoints.
- Se implementa un patrón de repositorio para el acceso a datos.
- Las migraciones de la base de datos se manejan automáticamente al iniciar la aplicación.

Para más detalles sobre la implementación de cada entidad, consulta los archivos correspondientes en la carpeta `Funcionalidades