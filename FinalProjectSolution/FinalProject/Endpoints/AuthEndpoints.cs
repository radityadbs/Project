using FinalProject.Dtos;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        // ================= REGISTER =================
        app.MapPost("/api/register", async (
        RegisterDto dto,
        AuthService service) =>
            {
                try
                {
                    await service.RegisterAsync(dto);
                    return Results.Ok("Registrasi berhasil. Silakan cek email.");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message); // 🔥
                }
            });

        // ================= VERIFY EMAIL =================
        app.MapGet("/api/verify-email", async (
            string token,
            AppDbContext db) =>
        {
            var verification = await db.EmailVerificationTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            if (verification == null || verification.ExpiredAt < DateTime.UtcNow)
            {
                return Results.Redirect("/login?error=Token tidak valid atau sudah expired");
            }

            var user = await db.Users.FindAsync(verification.UserId);
            if (user == null)
            {
                return Results.Redirect("/login?error=User tidak ditemukan");
            }

            user.IsEmailVerified = true;
            user.EmailVerifiedAt = DateTime.UtcNow.AddHours(7);

            db.EmailVerificationTokens.Remove(verification);
            await db.SaveChangesAsync();

            // REDIRECT KE LOGIN + PESAN
            return Results.Redirect("/login?success=Email berhasil diverifikasi, silakan login");
        });


        // // ================= LOGIN =================
        // app.MapPost("/api/login", async (LoginDto dto, AppDbContext db) =>
        // {
        //     var user = await db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

        //     if (user == null)
        //         return Results.BadRequest("User tidak ditemukan");

        //     if (!user.IsEmailVerified)
        //         return Results.BadRequest("Email belum diverifikasi");

        //     if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        //         return Results.BadRequest("Password salah");

        //     return Results.Ok(new LoginResponseDto
        //     {
        //         Email = user.Email
        //     });
        // });
    }
}
