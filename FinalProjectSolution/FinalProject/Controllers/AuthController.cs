using FinalProject.Dtos;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Services;
using FinalProject.Dtos;


namespace FinalProject.Controllers;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly UserLogService _logService;

    public AuthController(AppDbContext db, UserLogService logService)
    {
        _db = db;
        _logService = logService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return Redirect("/login?error=Email tidak ditemukan");

        if (!user.IsEmailVerified)
            return Redirect("/login?error=Email belum diverifikasi");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Redirect("/login?error=Password salah");

        //  SET SESSION
        HttpContext.Session.SetString("AUTH_EMAIL", user.Email);
        HttpContext.Session.SetString("AUTH_USERID", user.Id.ToString());

        //  LOG LOGIN
        _db.UserLogs.Add(new UserLog
        {
            Email = user.Email,
            Action = "LOGIN"
        });
        await _db.SaveChangesAsync();

        return Redirect("/");
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var email = HttpContext.Session.GetString("AUTH_EMAIL");

        if (email != null)
        {
            //  LOG LOGOUT
            _db.UserLogs.Add(new UserLog
            {
                Email = email,
                Action = "LOGOUT"
            });
            await _db.SaveChangesAsync();
        }

        //  CLEAR SESSION
        HttpContext.Session.Clear();
        return Redirect("/login?success=Berhasil logout");
    }


}
