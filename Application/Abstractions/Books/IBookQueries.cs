using Application.DTOs.Books;

namespace Application.Abstractions.Books;

public interface IBookQueries
{
    Task<List<BookResponse>> GetBooksAsync(int page, int pageSize, int? authorId, string? title, string? sortBy, CancellationToken ct);
    Task<BookResponse?> GetBookByIdAsync(int id, CancellationToken ct);
}
