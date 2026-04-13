namespace Frontend.Models
{
    public class LoanModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedDate { get; set; }
    }

    public class CreateLoanRequest
    {
        public int BookId { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class ReturnLoanRequest
    {
        public int Id { get; set; }
    }
}
