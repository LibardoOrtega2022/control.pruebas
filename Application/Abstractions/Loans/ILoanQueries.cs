using Application.DTOs.Loans;

namespace Application.Abstractions.Loans;

public interface ILoanQueries
{
    Task<List<LoanResponse>> GetLoansAsync(int page, int pageSize, string? status, int? bookId, CancellationToken ct);
    Task<LoanResponse?> GetLoanByIdAsync(int id, CancellationToken ct);
}
