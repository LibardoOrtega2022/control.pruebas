namespace Application.DTOs.Reports;

public class TopAuthorsReportDto
{
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public int TotalPages { get; set; }
    public int BookCount { get; set; }
}

public class AuthorWithoutBooksDto
{
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class AveragePagesByBookDto
{
    public decimal AveragePages { get; set; }
    public int TotalBooks { get; set; }
}

public class TotalBooksByAuthorDto
{
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public int BookCount { get; set; }
}

public class LibrarySummaryReportDto
{
    public List<TopAuthorsReportDto> Top5AuthorsByPages { get; set; } = new();
    public List<AuthorWithoutBooksDto> AuthorsWithoutBooks { get; set; } = new();
    public AveragePagesByBookDto AveragePages { get; set; } = new();
    public List<TotalBooksByAuthorDto> TotalBooksByAuthor { get; set; } = new();
}
