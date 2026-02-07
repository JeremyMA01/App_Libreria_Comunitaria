using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Rol { get; set; } = "Usuario";

    public bool Active { get; set; } = true;
}
