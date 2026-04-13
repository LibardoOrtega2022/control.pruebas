using Application.Abstractions.Reports;
using Application.DTOs.Reports;

namespace Core.Domains.Reports;

/// <summary>
/// Genera un resumen completo de la librería para dashboards/análisis
/// </summary>
public class GenerateLibrarySummaryReportDomain(IReportQueries reportQueries)
{
    public async Task<LibrarySummaryReportDto> GetSummaryAsync(CancellationToken ct)
    {
        // ⚠️ Ejecutar SECUENCIALMENTE (no en paralelo) porque la conexión no soporta MultipleActiveResultSets
        var top5 = await reportQueries.GetTop5AuthorsByPagesAsync(ct);
        var withoutBooks = await reportQueries.GetAuthorsWithoutBooksAsync(ct);
        var avgPages = await reportQueries.GetAveragePagesPerBookAsync(ct);
        var totalBooks = await reportQueries.GetTotalBooksByAuthorAsync(ct);

        return new LibrarySummaryReportDto
        {
            Top5AuthorsByPages = top5,
            AuthorsWithoutBooks = withoutBooks,
            AveragePages = avgPages,
            TotalBooksByAuthor = totalBooks
        };
    }
}
