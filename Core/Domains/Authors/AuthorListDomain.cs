using Application.Abstractions.Authors;
using Application.DTOs.Authors;

namespace Core.Domains.Authors;

public class AuthorListDomain(IAuthorQueries authorQueries) 
{
    public async Task<List<AuthorResponse>> GetAuthorsAsync(int page, int pageSize, string? sortBy, CancellationToken ct)
    {
        return await authorQueries.GetAuthorsAsync(page, pageSize, sortBy, ct);
    }
}