using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmailWorker;

public class EmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var host = _config["Email:SmtpHost"];
        var port = int.Parse(_config["Email:SmtpPort"]!);
        var username = _config["Email:Username"];
        var password = _config["Email:Password"];
        var from = _config["Email:From"];
        var fromName = _config["Email:FromName"] ?? "No-Reply";

        Console.WriteLine("=== EMAIL CONFIG ===");
        Console.WriteLine($"Host: {host}");
        Console.WriteLine($"Port: {port}");
        Console.WriteLine($"User: {username}");
        Console.WriteLine($"From: {from}");
        Console.WriteLine("====================");

        var message = new MailMessage
        {
            From = new MailAddress(from!, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);

        using var smtp = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        _logger.LogInformation("Sending email to {Email}", to);

        await smtp.SendMailAsync(message);

        _logger.LogInformation("Email sent successfully to {Email}", to);
    }

    public async Task SendVerificationEmail(string to, string link)
    {
        var subject = "Verifikasi Email Anda";
        var body = $"""
        <h3>Verifikasi Akun</h3>
        <p>Klik link berikut untuk verifikasi:</p>
        <a href="{link}">{link}</a>
        """;

        await SendAsync(to, subject, body);
    }
}
