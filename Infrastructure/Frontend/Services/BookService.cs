using Frontend.Models;
using System.Text;
using System.Text.Json;

namespace Frontend.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "http://localhost:5088/api/book";

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResponse<BookModel>?> GetBooksAsync(int page = 1, int pageSize = 10, int? authorId = null, string? title = null)
        {
            try
            {
                var url = $"{ApiUrl}?page={page}&pageSize={pageSize}";
                if (authorId.HasValue)
                    url += $"&authorId={authorId}";
                if (!string.IsNullOrEmpty(title))
                    url += $"&title={Uri.EscapeDataString(title)}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PaginatedResponse<BookModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener libros: {ex.Message}", ex);
            }
        }

        public async Task<BookModel?> GetBookByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrl}/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BookModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener libro: {ex.Message}", ex);
            }
        }

        public async Task<BookModel?> CreateBookAsync(CreateBookRequest request, Stream? imageStream = null, string? fileName = null)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(request.Title), "title");
                    content.Add(new StringContent(request.NumberOfPages.ToString()), "numberOfPages");
                    content.Add(new StringContent(request.AuthorId.ToString()), "authorId");

                    if (!string.IsNullOrEmpty(request.Genre))
                        content.Add(new StringContent(request.Genre), "genre");
                    if (request.PublishedDate.HasValue)
                        content.Add(new StringContent(request.PublishedDate.Value.ToString("yyyy-MM-dd")), "publishedDate");
                    if (!string.IsNullOrEmpty(request.ISBN))
                        content.Add(new StringContent(request.ISBN), "isbn");

                    if (imageStream != null && !string.IsNullOrEmpty(fileName))
                    {
                        content.Add(new StreamContent(imageStream), "file", fileName);
                    }

                    var response = await _httpClient.PostAsync(ApiUrl, content);
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<BookModel>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al crear libro: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateBookAsync(UpdateBookRequest request, Stream? imageStream = null, string? fileName = null)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(request.Id.ToString()), "id");
                    content.Add(new StringContent(request.Title), "title");
                    content.Add(new StringContent(request.NumberOfPages.ToString()), "numberOfPages");
                    content.Add(new StringContent(request.AuthorId.ToString()), "authorId");

                    if (!string.IsNullOrEmpty(request.Genre))
                        content.Add(new StringContent(request.Genre), "genre");
                    if (request.PublishedDate.HasValue)
                        content.Add(new StringContent(request.PublishedDate.Value.ToString("yyyy-MM-dd")), "publishedDate");
                    if (!string.IsNullOrEmpty(request.ISBN))
                        content.Add(new StringContent(request.ISBN), "isbn");

                    if (imageStream != null && !string.IsNullOrEmpty(fileName))
                    {
                        content.Add(new StreamContent(imageStream), "file", fileName);
                    }

                    var response = await _httpClient.PutAsync($"{ApiUrl}/{request.Id}", content);
                    response.EnsureSuccessStatusCode();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al actualizar libro: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al eliminar libro: {ex.Message}", ex);
            }
        }
    }
}
