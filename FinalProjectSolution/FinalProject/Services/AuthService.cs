using FinalProject.Dtos;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Shared;

namespace FinalProject.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConnection _rabbit;

    public AuthService(AppDbContext db, IConnection rabbit)
    {
        _db = db;
        _rabbit = rabbit;
    }

    /* =====================================================
     * REGISTER
     * ===================================================== */

    public async Task RegisterAsync(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Email sudah terdaftar");

        if (await _db.Users.AnyAsync(u => u.Nik == dto.Nik))
            throw new Exception("NIK sudah terdaftar");

        var user = new User
        {
            Email = dto.Email,
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

            Nik = dto.Nik,
            FullName = dto.FullName,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            DesaId = dto.DesaId,
            DesaName = dto.DesaName,
            IsEmailVerified = false
        };
        Console.WriteLine(dto.DesaId);
        Console.WriteLine(dto.DesaName);


        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = Guid.NewGuid().ToString();

        _db.EmailVerificationTokens.Add(new EmailVerificationToken
        {
            UserId = user.Id,
            Token = token,
            ExpiredAt = DateTime.UtcNow.AddHours(2)
        });

        await _db.SaveChangesAsync();

        /* ================= RABBITMQ ================= */

        try
        {
            using var channel = _rabbit.CreateModel();

            channel.QueueDeclare(
                queue: "email_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = new EmailVerificationMessage
            {
                Email = user.Email,
                Token = token
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(
                exchange: "",
                routingKey: "email_queue",
                basicProperties: null,
                body: body);
        }
        catch (Exception ex)
        {
            // LOG SAJA, JANGAN GAGALKAN REGISTER
            Console.WriteLine("RabbitMQ error: " + ex.Message);
        }

    }
}
