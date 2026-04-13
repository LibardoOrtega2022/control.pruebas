using Application.DTOs.Reports;

namespace Application.Abstractions.Reports;

public interface IReportQueries
{
    Task<List<TopAuthorDto>> GetTop5AuthorsByPagesAsync(CancellationToken ct);
    Task<List<AuthorWithoutBooksDto>> GetAuthorsWithoutBooksAsync(CancellationToken ct);
    Task<decimal> GetAveragePagesPerBookAsync(CancellationToken ct);
    Task<List<TotalBooksByAuthorDto>> GetTotalBooksByAuthorAsync(CancellationToken ct);
}
