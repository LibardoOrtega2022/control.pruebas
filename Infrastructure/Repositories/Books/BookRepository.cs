using Application.Abstractions.Books;
using Application.Entities;
using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Books;

public class BookRepository(AppDbContext dbContext) : IBookRepository
{
    public async Task AddBookAsync(BookEntity book, CancellationToken ct)
    {
        book.CreatedDate = DateTime.UtcNow;
        await dbContext.BooksEntities.AddAsync(book, ct);
    }

    public async Task<BookEntity?> GetBookByIdAsync(int id, CancellationToken ct)
    {
        return await dbContext.BooksEntities
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken: ct);
    }

    public async Task UpdateBookAsync(BookEntity book, CancellationToken ct)
    {
        book.UpdatedDate = DateTime.UtcNow;
        dbContext.BooksEntities.Update(book);
        await Task.CompletedTask;
    }

    public async Task DeleteBookAsync(int id, CancellationToken ct)
    {
        var book = await GetBookByIdAsync(id, ct);
        if (book != null)
        {
            book.IsDeleted = true;
            book.UpdatedDate = DateTime.UtcNow;
            dbContext.BooksEntities.Update(book);
        }
    }
}
