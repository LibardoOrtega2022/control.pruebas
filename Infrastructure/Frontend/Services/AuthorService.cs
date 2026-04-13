using Frontend.Models;
using System.Text;
using System.Text.Json;

namespace Frontend.Services
{
    public class AuthorService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "http://localhost:5088/api/author";

        public AuthorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResponse<AuthorModel>?> GetAuthorsAsync(int page = 1, int pageSize = 10, string? sortBy = null)
        {
            try
            {
                var url = $"{ApiUrl}?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(sortBy))
                    url += $"&sortBy={sortBy}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PaginatedResponse<AuthorModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener autores: {ex.Message}", ex);
            }
        }

        public async Task<AuthorModel?> GetAuthorByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrl}/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AuthorModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener autor: {ex.Message}", ex);
            }
        }

        public async Task<AuthorModel?> CreateAuthorAsync(CreateAuthorRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AuthorModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al crear autor: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAuthorAsync(UpdateAuthorRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{ApiUrl}/{request.Id}", content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al actualizar autor: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al eliminar autor: {ex.Message}", ex);
            }
        }
    }
}
