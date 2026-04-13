using Application.Entities;

namespace Application.Abstractions.Authors;

public interface IAuthorRepository
{
    Task AddAuthorAsync(AuthorEntity author, CancellationToken ct);
    Task<AuthorEntity?> GetAuthorByIdAsync(int id, CancellationToken ct);
    Task UpdateAuthorAsync(AuthorEntity author, CancellationToken ct);
    Task DeleteAuthorAsync(int id, CancellationToken ct);
}