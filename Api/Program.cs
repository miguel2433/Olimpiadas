// Importaciones necesarias para el funcionamiento del programa
using Api.Funcionalidades;
using Api.Persistencia;
using Carter;
using Microsoft.EntityFrameworkCore;

// Crear el constructor de la aplicación web
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor de dependencias
builder.Services.AddControllers(); // Agregar soporte para controladores
builder.Services.AddEndpointsApiExplorer(); // Agregar explorador de endpoints de API
builder.Services.AddSwaggerGen(); // Agregar generador de documentación Swagger

// Agregar servicio de autorización
builder.Services.AddAuthorization();

// Agregar servicios personalizados y Carter para el enrutamiento
builder.Services.AddServicesManager();
builder.Services.AddCarter();

// Obtener la cadena de conexión de la configuración
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Detectar automáticamente la versión de MySQL
var mySqlVersion = ServerVersion.AutoDetect(connectionString);

// Configurar el contexto de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, mySqlVersion));

// Crear opciones para el contexto de la base de datos
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseMySql(connectionString, mySqlVersion)
    .Options;

// Crear una instancia del contexto de la base de datos
var context = new AppDbContext(options);

// Asegurar que la base de datos esté creada
context.Database.EnsureCreated();

// Construir la aplicación
var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger en entorno de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapear las rutas definidas con Carter
app.MapCarter();

// Redirigir HTTP a HTTPS
app.UseHttpsRedirection();

// Habilitar la autorización
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// Iniciar la aplicación
app.Run();