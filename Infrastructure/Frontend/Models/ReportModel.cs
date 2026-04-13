namespace Frontend.Models
{
    public class ReportSummary
    {
        public List<TopAuthorDto> TopAuthors { get; set; } = new();
        public List<AuthorWithoutBooksDto> AuthorsWithoutBooks { get; set; } = new();
        public decimal AveragePagesPerBook { get; set; }
        public List<TotalBooksByAuthorDto> TotalBooksByAuthor { get; set; } = new();
    }

    public class TopAuthorDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int TotalPages { get; set; }
    }

    public class AuthorWithoutBooksDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }

    public class TotalBooksByAuthorDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int TotalBooks { get; set; }
    }
}
