using Frontend.Models;
using System.Text.Json;

namespace Frontend.Services
{
    public class ReportService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "http://localhost:5088/api/report/summary";

        public ReportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ReportSummary?> GetSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ReportSummary>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener reportes: {ex.Message}", ex);
            }
        }
    }
}
