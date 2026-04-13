using Application.Entities;

namespace Application.Abstractions.Books;

public interface IBookRepository
{
    Task AddBookAsync(BookEntity book, CancellationToken ct);
    Task<BookEntity?> GetBookByIdAsync(int id, CancellationToken ct);
    Task UpdateBookAsync(BookEntity book, CancellationToken ct);
    Task DeleteBookAsync(int id, CancellationToken ct);
}
