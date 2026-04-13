using Application.Abstractions.Loans;
using Application.DTOs.Loans;

namespace Core.Domains.Loans;

public class GetLoansDomain(ILoanQueries loanQueries)
{
    public async Task<List<LoanResponse>> GetLoansAsync(
        int page,
        int pageSize,
        string? status,
        int? bookId,
        CancellationToken ct)
    {
        return await loanQueries.GetLoansAsync(page, pageSize, status, bookId, ct);
    }
}
