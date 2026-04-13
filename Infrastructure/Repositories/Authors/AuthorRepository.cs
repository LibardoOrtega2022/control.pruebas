using Application.Abstractions.Authors;
using Application.Entities;
using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Authors;

public class AuthorRepository(AppDbContext dbContext) : IAuthorRepository
{
    public async Task AddAuthorAsync(AuthorEntity author, CancellationToken ct)
    {
        author.CreatedDate = DateTime.UtcNow;
        await dbContext.AuthorEntities.AddAsync(author, ct);
    }

    public async Task<AuthorEntity?> GetAuthorByIdAsync(int id, CancellationToken ct)
    {
        return await dbContext.AuthorEntities
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken: ct);
    }

    public async Task UpdateAuthorAsync(AuthorEntity author, CancellationToken ct)
    {
        author.UpdatedDate = DateTime.UtcNow;
        dbContext.AuthorEntities.Update(author);
        await Task.CompletedTask;
    }

    public async Task DeleteAuthorAsync(int id, CancellationToken ct)
    {
        var author = await GetAuthorByIdAsync(id, ct);
        if (author != null)
        {
            author.IsDeleted = true;
            author.UpdatedDate = DateTime.UtcNow;
            dbContext.AuthorEntities.Update(author);
        }
    }
}