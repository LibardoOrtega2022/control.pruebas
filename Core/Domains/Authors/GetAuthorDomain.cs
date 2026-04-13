using Application.Abstractions.Authors;
using Application.DTOs.Authors;

namespace Core.Domains.Authors;

public class GetAuthorDomain(IAuthorQueries authorQueries)
{
    public async Task<AuthorResponse?> GetByIdAsync(int id, CancellationToken ct)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));

        return await authorQueries.GetAuthorByIdAsync(id, ct);
    }
}
