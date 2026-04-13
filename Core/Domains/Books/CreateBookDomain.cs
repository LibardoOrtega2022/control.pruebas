using Application.Abstractions;
using Application.Abstractions.Authors;
using Application.Abstractions.Books;
using Application.DTOs.Books;
using Application.Entities;

namespace Core.Domains.Books;

public class CreateBookDomain(
    IBookRepository repo,
    IAuthorRepository authorRepo,
    IUnitOfWork uow)
{
    public async Task<BookResponse> CreateAsync(CreateBookRequest request, string? coverImagePath, CancellationToken ct)
    {
        // Validar que el autor exista
        var author = await authorRepo.GetAuthorByIdAsync(request.AuthorId, ct);
        if (author == null)
            throw new KeyNotFoundException($"Autor con ID {request.AuthorId} no encontrado");

        // Validar fechas
        if (request.PublishedDate > DateTime.UtcNow)
            throw new ArgumentException("La fecha de publicación no puede ser en el futuro");

        // Crear entidad
        var book = new BookEntity
        {
            Title = request.Title,
            NumberOfPages = request.NumberOfPages,
            AuthorId = request.AuthorId,
            Genre = request.Genre,
            PublishedDate = request.PublishedDate,
            ISBN = request.ISBN,
            CoverImagePath = coverImagePath,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = null, // Inicializar explícitamente como null
            IsDeleted = false
        };

        await uow.BeginAsync(ct);

        try
        {
            await repo.AddBookAsync(book, ct);
            await uow.CommitAsync(ct);

            return new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                NumberOfPages = book.NumberOfPages,
                AuthorId = book.AuthorId,
                AuthorName = $"{author.Name} {author.LastName}",
                Genre = book.Genre,
                PublishedDate = book.PublishedDate,
                ISBN = book.ISBN,
                CoverImagePath = book.CoverImagePath,
                CreatedDate = book.CreatedDate,
                UpdatedDate = book.UpdatedDate
            };
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
