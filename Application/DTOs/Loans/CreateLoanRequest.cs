namespace Application.DTOs.Loans;

public class CreateLoanRequest
{
    public int BookId { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
}
