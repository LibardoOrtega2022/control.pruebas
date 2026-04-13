namespace Application.DTOs.Authors;

public class AuthorResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Country { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int BookCount { get; set; }
}
