using Application.Abstractions;
using Application.Abstractions.Loans;
using Application.DTOs.Loans;

namespace Core.Domains.Loans;

public class ReturnLoanDomain(
    ILoanRepository repo,
    ILoanQueries queries,
    IUnitOfWork uow)
{
    public async Task<LoanResponse> ReturnBookAsync(int loanId, CancellationToken ct)
    {
        if (loanId <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(loanId));

        await uow.BeginAsync(ct);

        try
        {
            var loan = await repo.GetLoanByIdAsync(loanId, ct);
            if (loan == null)
                throw new KeyNotFoundException($"Préstamo con ID {loanId} no encontrado");

            if (loan.ReturnDate != null)
                throw new InvalidOperationException("Este préstamo ya fue devuelto");

            // Marcar como devuelto y calcular estado
            loan.ReturnDate = DateTime.UtcNow;
            loan.Status = loan.ReturnDate > loan.DueDate ? "Overdue" : "Returned";

            await repo.UpdateLoanAsync(loan, ct);
            await uow.CommitAsync(ct);

            var updated = await queries.GetLoanByIdAsync(loanId, ct);
            return updated ?? throw new Exception("Error al obtener el préstamo actualizado");
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
