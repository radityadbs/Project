using Microsoft.AspNetCore.Http;

namespace FinalProject.Services;

public class AuthSessionService
{
    private readonly IHttpContextAccessor _http;

    private const string SESSION_EMAIL_KEY = "AUTH_EMAIL";

    public AuthSessionService(IHttpContextAccessor http)
    {
        _http = http;
    }

    public bool IsLoggedIn =>
        _http.HttpContext?.Session.GetString(SESSION_EMAIL_KEY) != null;

    public string? Email =>
        _http.HttpContext?.Session.GetString(SESSION_EMAIL_KEY);

    public void Logout()
    {
        _http.HttpContext?.Session.Clear();
    }
}
