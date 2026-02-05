using App_Libreria_Comunitaria.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//agregado
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configuración de la conexión a la base de datos
builder.Services.AddDbContext<BookContexts>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //agregado
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseAuthorization();

app.MapControllers();

app.Run();
