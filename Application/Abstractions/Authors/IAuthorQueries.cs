using Application.DTOs.Authors;

namespace Application.Abstractions.Authors;

public interface IAuthorQueries
{
    Task<List<AuthorResponse>> GetAuthorsAsync(int page, int pageSize, string? sortBy, CancellationToken ct);
    Task<AuthorResponse?> GetAuthorByIdAsync(int id, CancellationToken ct);
}