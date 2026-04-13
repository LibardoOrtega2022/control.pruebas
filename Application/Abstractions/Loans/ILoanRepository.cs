using Application.Entities;

namespace Application.Abstractions.Loans;

public interface ILoanRepository
{
    Task AddLoanAsync(LoanEntity loan, CancellationToken ct);
    Task<LoanEntity?> GetLoanByIdAsync(int id, CancellationToken ct);
    Task UpdateLoanAsync(LoanEntity loan, CancellationToken ct);
    Task<LoanEntity?> GetActiveLoanByBookIdAsync(int bookId, CancellationToken ct);
}
