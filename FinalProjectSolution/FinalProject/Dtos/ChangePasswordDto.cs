using System.ComponentModel.DataAnnotations;

namespace FinalProject.Dtos;

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = "";

    [Required, MinLength(6)]
    public string NewPassword { get; set; } = "";

    [Required, Compare("NewPassword", ErrorMessage = "Konfirmasi password tidak cocok")]
    public string ConfirmPassword { get; set; } = "";
}

