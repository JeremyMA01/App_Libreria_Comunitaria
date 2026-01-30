using CodeFirstClases.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//agregamos soporte para swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configuración de la conexión a la base de datos
builder.Services.AddDbContext<MoviesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.AllowAnyOrigin() //builder.WithOrigins("http://localhost:4200") // URL de tu proyecto Angular
            .AllowAnyMethod()// Permite cualquier método HTTP (GET, POST, etc.)
            .AllowAnyHeader());// Permite cualquier encabezado
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //agregado
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
