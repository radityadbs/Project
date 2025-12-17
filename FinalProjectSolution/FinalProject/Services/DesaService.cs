using System.Net.Http.Headers;
using System.Text.Json;
using FinalProject.Dtos;

namespace FinalProject.Services;

public class DesaService
{
    private readonly HttpClient _http;
    private string? _token;

    public DesaService(HttpClient http)
    {
        _http = http;
    }

    // ================= GET TOKEN =================
    private async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_token))
            return _token;

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "index.php/svc/get_token"
        );

        // BASIC AUTH (WAJIB)
        var basic = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes("operator4@oti.com:abc")
        );

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Basic", basic);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<TokenResponse>();
        _token = json!.token;

        return _token!;
    }

    // ================= SEARCH DESA =================
    public async Task<List<DesaDto>> SearchAsync(string query)
    {
        var token = await GetTokenAsync();

        var form = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("search", query),
            new KeyValuePair<string, string>("rows", "10")
        });

        var response = await _http.PostAsync(
            $"index.php/svc/get_desa/{token}",
            form
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DesaApiResponse>();
        return result?.rows ?? new();
    }

    // ================= GET DESA BY ID =================
    public async Task<DesaDto?> GetDesaByNameAsync(string desaName)
    {
        var token = await GetTokenAsync();

        var form = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("rows", "1"),
            new KeyValuePair<string, string>("search", desaName)
        });

        var response = await _http.PostAsync(
            $"index.php/svc/get_desa/{token}",
            form
        );

        var result = await response.Content.ReadFromJsonAsync<DesaApiResponse>();
        return result?.rows.FirstOrDefault(x => x.id == desaName);
    }
}
