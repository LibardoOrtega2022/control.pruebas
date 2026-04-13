namespace Application.DTOs.Books;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public int NumberOfPages { get; set; }
    public int AuthorId { get; set; }
    public string Genre { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string? ISBN { get; set; }
    // La imagen viene en un campo "file" del multipart/form-data
}
