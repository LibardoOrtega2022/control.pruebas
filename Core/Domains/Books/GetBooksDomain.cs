using Application.Abstractions.Books;
using Application.DTOs.Books;

namespace Core.Domains.Books;

public class GetBooksDomain(IBookQueries bookQueries)
{
    public async Task<List<BookResponse>> GetBooksAsync(
        int page,
        int pageSize,
        int? authorId,
        string? title,
        string? sortBy,
        CancellationToken ct)
    {
        return await bookQueries.GetBooksAsync(page, pageSize, authorId, title, sortBy, ct);
    }
}
