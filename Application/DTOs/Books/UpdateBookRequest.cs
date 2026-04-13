namespace Application.DTOs.Books;

public class UpdateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public int NumberOfPages { get; set; }
    public string Genre { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string? ISBN { get; set; }
}
