using Application.Abstractions;
using Application.Abstractions.Authors;

namespace Core.Domains.Authors;

public class DeleteAuthorDomain(
    IAuthorRepository repo,
    IUnitOfWork uow)
{
    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));

        await uow.BeginAsync(ct);

        try
        {
            var author = await repo.GetAuthorByIdAsync(id, ct);
            if (author == null)
                throw new KeyNotFoundException($"Autor con ID {id} no encontrado");

            await repo.DeleteAuthorAsync(id, ct);
            await uow.CommitAsync(ct);
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
