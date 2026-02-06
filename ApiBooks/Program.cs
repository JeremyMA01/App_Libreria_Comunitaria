using App_Libreria_Comunitaria.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configuración de la conexión a la base de datos
builder.Services.AddDbContext<LibreriaContexts>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.AllowAnyOrigin() //Cualquier método URl
    .AllowAnyMethod() //Cualquier método GET - POST
    .AllowAnyHeader()); // Cualquier encabezado
}
);

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
