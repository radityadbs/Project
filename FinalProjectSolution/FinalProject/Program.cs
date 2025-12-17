using System.Text;
using System.Text.Json;
using FinalProject.Components;
using FinalProject.Endpoints;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Shared;
using RabbitConnectionFactory = RabbitMQ.Client.ConnectionFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.AddControllers();

builder.Services.AddSingleton<IConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var host = config["RabbitMQ:HostName"];
    var port = int.Parse(config["RabbitMQ:Port"] ?? "5672");

    var factory = new ConnectionFactory
    {
        HostName = host,
        Port = port,
        UserName = "guest",
        Password = "guest"
    };

    return factory.CreateConnection();
});


builder.Services.AddHttpContextAccessor();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpClient<DesaService>(client =>
{
    client.BaseAddress = new Uri("http://api.inixindojogja.com/");
});


builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5215/");
});


builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthSessionService>();
builder.Services.AddScoped<UserLogService>();

// builder.Services.AddScoped<DesaService>();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery(); // ⬅️ PENTING: DI SINI

app.MapControllers();
app.MapAuthEndpoints();
app.MapDesaEndpoints();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
