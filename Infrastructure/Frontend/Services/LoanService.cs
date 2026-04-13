using Frontend.Models;
using System.Text;
using System.Text.Json;

namespace Frontend.Services
{
    public class LoanService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "http://localhost:5088/api/loan";

        public LoanService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResponse<LoanModel>?> GetLoansAsync(int page = 1, int pageSize = 10, string? status = null)
        {
            try
            {
                var url = $"{ApiUrl}?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(status))
                    url += $"&status={status}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PaginatedResponse<LoanModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener préstamos: {ex.Message}", ex);
            }
        }

        public async Task<LoanModel?> GetLoanByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrl}/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoanModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener préstamo: {ex.Message}", ex);
            }
        }

        public async Task<LoanModel?> CreateLoanAsync(CreateLoanRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiUrl, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        throw new ApplicationException("Este libro ya tiene un préstamo activo");
                    throw new ApplicationException($"Error: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoanModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al crear préstamo: {ex.Message}", ex);
            }
        }

        public async Task<bool> ReturnLoanAsync(int id)
        {
            try
            {
                var json = JsonSerializer.Serialize(new { id });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{ApiUrl}/{id}/return", content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al devolver préstamo: {ex.Message}", ex);
            }
        }
    }
}
