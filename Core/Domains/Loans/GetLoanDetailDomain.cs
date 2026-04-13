using Application.Abstractions.Loans;
using Application.DTOs.Loans;

namespace Core.Domains.Loans;

public class GetLoanDetailDomain(ILoanQueries loanQueries)
{
    public async Task<LoanResponse?> GetByIdAsync(int id, CancellationToken ct)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));

        return await loanQueries.GetLoanByIdAsync(id, ct);
    }
}
