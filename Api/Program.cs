using Api.Funcionalidades;
using Api.Persistencia;
using Carter;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddAuthorization();

builder.Services.AddServicesManager();
builder.Services.AddCarter();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var mySqlVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, mySqlVersion));

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseMySql(connectionString, mySqlVersion)
    .Options;

var context = new AppDbContext(options);

context.Database.EnsureCreated();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();