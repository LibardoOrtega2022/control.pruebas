namespace Frontend.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int NumberOfPages { get; set; }
        public string? Genre { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? ISBN { get; set; }
        public string? CoverImagePath { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class CreateBookRequest
    {
        public string Title { get; set; } = string.Empty;
        public int NumberOfPages { get; set; }
        public int AuthorId { get; set; }
        public string? Genre { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? ISBN { get; set; }
    }

    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int NumberOfPages { get; set; }
        public int AuthorId { get; set; }
        public string? Genre { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string? ISBN { get; set; }
    }
}
