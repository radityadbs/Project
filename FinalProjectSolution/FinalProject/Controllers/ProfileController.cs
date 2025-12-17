using FinalProject.Dtos;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly UserLogService _log;


    public ProfileController(AppDbContext db, IWebHostEnvironment env, UserLogService log)
    {
        Console.WriteLine("PROFILE CONTROLLER CREATED");
        _db = db;
        _log = log;
        _env = env;
    }

    // ================= GET PROFILE =================
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var email = HttpContext.Session.GetString("AUTH_EMAIL");
        if (email == null)
            return Unauthorized();

        var user = await _db.Users.FirstAsync(x => x.Email == email);

        return Ok(new ResProfileDto
        {
            Nik = user.Nik,
            FullName = user.FullName,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            DesaId = user.DesaId,
            DesaName = user.DesaName,
            PhotoPath = user.PhotoPath
        });
    }

    // ================= UPDATE PROFILE =================
    [HttpPost]
    public async Task<IActionResult> UpdateProfile([FromForm] ReqProfileDto dto)
    {
        var email = HttpContext.Session.GetString("AUTH_EMAIL");
        if (email == null)
            return Unauthorized();

        var user = await _db.Users.FirstAsync(x => x.Email == email);

        user.Nik = dto.Nik;
        user.FullName = dto.FullName;
        user.Address = dto.Address;
        user.PhoneNumber = dto.PhoneNumber;
        user.DesaId = dto.DesaId;
        user.DesaName = dto.DesaName;

        Console.WriteLine(dto.DesaId);
        Console.WriteLine(dto.DesaName);

        // ===== HANDLE UPLOAD FOTO =====
        if (dto.Photo != null && dto.Photo.Length > 0)
        {
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
            var filePath = Path.Combine(uploads, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.Photo.CopyToAsync(stream);

            user.PhotoPath = $"/uploads/{fileName}";
        }

        await _db.SaveChangesAsync();
        await _log.LogAsync(user.Email, "UPDATE_PROFILE");
        return Redirect("/profile?success=Profil berhasil diperbarui");
    }


}
