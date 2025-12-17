using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Dtos;

public class ReqProfileDto
{
    [EmailAddress]
    public string Email { get; set; } = "";
    [StringLength(16, MinimumLength = 16)]
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

    public IFormFile? Photo { get; set; }

    public string? PhotoPath { get; set; }
}
