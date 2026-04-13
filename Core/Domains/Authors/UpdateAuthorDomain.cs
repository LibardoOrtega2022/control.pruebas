using Application.Abstractions;
using Application.Abstractions.Authors;
using Application.DTOs.Authors;
using Application.Entities;

namespace Core.Domains.Authors;

public class UpdateAuthorDomain(
    IAuthorRepository repo,
    IAuthorQueries queries,
    IUnitOfWork uow)
{
    public async Task<AuthorResponse> UpdateAsync(int id, UpdateAuthorRequest request, CancellationToken ct)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));

        await uow.BeginAsync(ct);

        try
        {
            var author = await repo.GetAuthorByIdAsync(id, ct);
            if (author == null)
                throw new KeyNotFoundException($"Autor con ID {id} no encontrado");

            // Actualizar propiedades
            author.Name = request.Name;
            author.LastName = request.LastName;
            author.BirthDate = request.BirthDate;
            author.Country = request.Country;
            author.Biography = request.Biography;

            await repo.UpdateAuthorAsync(author, ct);
            await uow.CommitAsync(ct);

            // Obtener el record actualizado
            var updated = await queries.GetAuthorByIdAsync(id, ct);
            return updated ?? throw new Exception("Error al obtener el registro actualizado");
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
