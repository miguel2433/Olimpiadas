using Api.Funcionalidades;
using Api.Persistencia;
using Carter;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization();

builder.Services.AddServices();
builder.Services.AddCarter();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
builder.Services.AddDbContext<AppDbContext>();
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
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