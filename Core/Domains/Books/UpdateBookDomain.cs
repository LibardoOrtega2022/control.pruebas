using Application.Abstractions;
using Application.Abstractions.Authors;
using Application.Abstractions.Books;
using Application.DTOs.Books;
using Application.Entities;

namespace Core.Domains.Books;

public class UpdateBookDomain(
    IBookRepository repo,
    IBookQueries queries,
    IUnitOfWork uow)
{
    public async Task<BookResponse> UpdateAsync(int id, UpdateBookRequest request, string? newCoverImagePath, CancellationToken ct)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));

        await uow.BeginAsync(ct);

        try
        {
            var book = await repo.GetBookByIdAsync(id, ct);
            if (book == null)
                throw new KeyNotFoundException($"Libro con ID {id} no encontrado");

            // Actualizar campos
            book.Title = request.Title;
            book.NumberOfPages = request.NumberOfPages;
            book.Genre = request.Genre;
            book.PublishedDate = request.PublishedDate;
            book.ISBN = request.ISBN;
            
            // Si hay nueva imagen, actualizar ruta
            if (newCoverImagePath != null)
                book.CoverImagePath = newCoverImagePath;

            await repo.UpdateBookAsync(book, ct);
            await uow.CommitAsync(ct);

            var updated = await queries.GetBookByIdAsync(id, ct);
            return updated ?? throw new Exception("Error al obtener el libro actualizado");
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
