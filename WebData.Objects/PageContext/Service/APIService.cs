using System.Net.Http.Json;

namespace WebData.Objects.PageContext.Service;

public class ApiService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<bool> CheckConncetion(string url)
    {
        var response = await _httpClient.GetAsync(url);

        return response.IsSuccessStatusCode;
    }

    public async Task<T> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
#pragma warning disable CS8603 // Mögliche Nullverweisrückgabe.
        return await response.Content.ReadFromJsonAsync<T>();
#pragma warning restore CS8603 // Mögliche Nullverweisrückgabe.
    }

    public async Task<T> PostAsync<T>(string url, object data)
    {
        var response = await _httpClient.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
#pragma warning disable CS8603 // Mögliche Nullverweisrückgabe.
        return await response.Content.ReadFromJsonAsync<T>();
#pragma warning restore CS8603 // Mögliche Nullverweisrückgabe.
    }
}
