using System.ComponentModel.DataAnnotations;

namespace FinalProject.Dtos;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Username { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    // ===== PROFILE DATA =====
    [Required, StringLength(16, MinimumLength = 16)]
    public string Nik { get; set; } = "";

    [Required]
    public string FullName { get; set; } = "";

    [Required]
    public string Address { get; set; } = "";

    [Required]
    public string PhoneNumber { get; set; } = "";

    [Required]
    public string DesaId { get; set; } = "";
    [Required]
    public string DesaName { get; set; } = "";
}
