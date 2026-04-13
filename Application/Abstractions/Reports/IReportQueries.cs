using Application.DTOs.Reports;

namespace Application.Abstractions.Reports;

public interface IReportQueries
{
    Task<List<TopAuthorsReportDto>> GetTop5AuthorsByPagesAsync(CancellationToken ct);
    Task<List<AuthorWithoutBooksDto>> GetAuthorsWithoutBooksAsync(CancellationToken ct);
    Task<AveragePagesByBookDto> GetAveragePagesPerBookAsync(CancellationToken ct);
    Task<List<TotalBooksByAuthorDto>> GetTotalBooksByAuthorAsync(CancellationToken ct);
}
