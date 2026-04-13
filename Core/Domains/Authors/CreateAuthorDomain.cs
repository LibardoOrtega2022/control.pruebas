using Application.Abstractions;
using Application.Abstractions.Authors;
using Application.DTOs.Authors;
using Application.Entities;

namespace Core.Domains.Authors;

public class CreateAuthorDomain(
    IAuthorRepository repo,
    IUnitOfWork uow)
{
    public async Task<AuthorResponse> CreateAsync(CreateAuthorRequest request, CancellationToken ct)
    {
        // Mapeo de DTO a Entity
        var author = new AuthorEntity
        {
            Name = request.Name,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            Country = request.Country,
            Biography = request.Biography,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = null, // Inicializar explícitamente como null
            IsDeleted = false
        };

        await uow.BeginAsync(ct);

        try
        {
            await repo.AddAuthorAsync(author, ct);
            await uow.CommitAsync(ct);

            // Devolver DTO
            return new AuthorResponse
            {
                Id = author.Id,
                Name = author.Name,
                LastName = author.LastName,
                BirthDate = author.BirthDate,
                Country = author.Country,
                Biography = author.Biography,
                CreatedDate = author.CreatedDate,
                UpdatedDate = author.UpdatedDate,
                BookCount = 0
            };
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}