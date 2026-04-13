using Application.Abstractions;
using Application.Abstractions.Books;

namespace Core.Domains.Books;

public class DeleteBookDomain(
    IBookRepository repo,
    IUnitOfWork uow)
{
    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));

        await uow.BeginAsync(ct);

        try
        {
            var book = await repo.GetBookByIdAsync(id, ct);
            if (book == null)
                throw new KeyNotFoundException($"Libro con ID {id} no encontrado");

            await repo.DeleteBookAsync(id, ct);
            await uow.CommitAsync(ct);
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
