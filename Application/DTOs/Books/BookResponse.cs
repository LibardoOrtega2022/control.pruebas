namespace Application.DTOs.Books;

public class BookResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int NumberOfPages { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string? ISBN { get; set; }
    public string? CoverImagePath { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
