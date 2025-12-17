using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

namespace EmailWorker;

public class Worker : BackgroundService
{
    private readonly EmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Worker> _logger;

    public Worker(
        EmailService emailService,
        IConfiguration configuration,
        ILogger<Worker> logger)
    {
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rabbitHost = _configuration["RabbitMQ:HostName"];

        var factory = new ConnectionFactory
        {
            HostName = rabbitHost,
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true
        };

        IConnection? connection = null;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Trying to connect to RabbitMQ at {Host}", rabbitHost);
                connection = factory.CreateConnection();
                _logger.LogInformation("RabbitMQ connected!");
                break;
            }
            catch
            {
                _logger.LogWarning("RabbitMQ not ready, retrying in 5 seconds...");
                await Task.Delay(5000, stoppingToken);
            }
        }

        if (connection == null) return;

        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "email_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Email received raw: {Message}", json);

                var data = JsonSerializer.Deserialize<EmailVerificationMessage>(json);

                if (data == null)
                {
                    _logger.LogError("Invalid email message format");
                    channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                var verifyLink = $"http://localhost:5000/api/verify-email?token={data.Token}";

                await _emailService.SendVerificationEmail(
                    data.Email,
                    verifyLink
                );

                channel.BasicAck(ea.DeliveryTag, false);
                _logger.LogInformation("Email sent & message ACKed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process email message");
                channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };


        channel.BasicConsume(
            queue: "email_queue",
            autoAck: false,
            consumer: consumer
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

}
