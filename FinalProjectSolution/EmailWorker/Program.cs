using EmailWorker;
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<EmailService>();
    })
    .Build();

await host.RunAsync();
