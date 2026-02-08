using App_Libreria_Comunitaria.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configuración de JWT con validación de nulidad (EVITA EL ERROR DE PANTALLA NEGRA)
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ClaveSecretaDeRespaldoDe32CaracteresMinimo!!"; // Si es nulo, usa esta
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "MiLibreriaApi",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "LibroStoreAngular",
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

// 3. Base de Datos
builder.Services.AddDbContext<LibreriaContexts>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// 5. Pipeline de HTTP (EL ORDEN AQUÍ ES CRÍTICO)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ⚠️ IMPORTANTE: El orden de estos middlewares debe ser exactamente este:
app.UseCors("AllowAngularApp"); // Primero permitir el origen

app.UseAuthentication();        // Segundo autenticar (¿quién eres?)
app.UseAuthorization();         // Tercero autorizar (¿qué puedes hacer?)

app.MapControllers();

app.Run();