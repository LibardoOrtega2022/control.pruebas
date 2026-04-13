using Application.Abstractions.Books;
using Application.DTOs.Books;

namespace Core.Domains.Books;

public class GetBookDetailDomain(IBookQueries bookQueries)
{
    public async Task<BookResponse?> GetByIdAsync(int id, CancellationToken ct)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));

        return await bookQueries.GetBookByIdAsync(id, ct);
    }
}
