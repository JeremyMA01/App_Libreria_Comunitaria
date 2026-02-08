using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App_Libreria_Comunitaria.Contexts;
using App_Libreria_Comunitaria.Models;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LibreriaContexts _context;
    private readonly IConfiguration _config;

    public AuthController(LibreriaContexts context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginModel login)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u =>
            u.Email == login.Email &&
            u.Password == login.Password &&
            u.Active);

        if (usuario == null)
            return Unauthorized("Credenciales incorrectas");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim("UserId", usuario.Id.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}