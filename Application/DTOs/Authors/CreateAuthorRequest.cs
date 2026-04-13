namespace Application.DTOs.Authors;

public class CreateAuthorRequest
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Country { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
}
