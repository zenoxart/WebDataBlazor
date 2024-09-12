using System.Net.Http.Json;
using WebData.Objects.PageContext.Utilities;

namespace WebData.Objects.PageContext.Service
{
    /// <summary>
    /// Definiert einen Kommunikations-Service um mit einer REST-API zu kommunizieren
    /// </summary>
    /// <param name="httpClient"></param>
    public class ApiService(HttpClient httpClient)
    {
        /// <summary>
        /// Definiert die Basis-URL zur API
        /// </summary>
        public string BaseURL { get; set; } = ConfigHandler.APIUrlBase;

        /// <summary>
        /// Definiert den HTTP/HTTPS-Client
        /// </summary>
        private readonly HttpClient _httpClient = httpClient;

        /// <summary>
        /// Überprüft ob die API-Adresse erreichbar ist
        /// </summary>
        public async Task<bool> CheckConncetion(string url)
        {
            var response = await _httpClient.GetAsync(BaseURL + url);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Läd das Objekt von der API-Adresse
        /// </summary>
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(BaseURL + url);
            response.EnsureSuccessStatusCode();
#pragma warning disable CS8603 // Mögliche Nullverweisrückgabe.
            return await response.Content.ReadFromJsonAsync<T>();
#pragma warning restore CS8603 // Mögliche Nullverweisrückgabe.
        }


        /// <summary>
        /// Sendet das Objekt an die API-Adresse
        /// </summary>
        public async Task<T> PostAsync<T>(string url, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseURL + url, data);
            response.EnsureSuccessStatusCode();
#pragma warning disable CS8603 // Mögliche Nullverweisrückgabe.
            return await response.Content.ReadFromJsonAsync<T>();
#pragma warning restore CS8603 // Mögliche Nullverweisrückgabe.
        }
    }
}