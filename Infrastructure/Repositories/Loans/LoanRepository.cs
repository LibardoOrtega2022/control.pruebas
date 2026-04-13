using Application.Abstractions.Loans;
using Application.Entities;
using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Loans;

public class LoanRepository(AppDbContext dbContext) : ILoanRepository
{
    public async Task AddLoanAsync(LoanEntity loan, CancellationToken ct)
    {
        loan.CreatedDate = DateTime.UtcNow;
        loan.Status = "Active";
        await dbContext.Loans.AddAsync(loan, ct);
    }

    public async Task<LoanEntity?> GetLoanByIdAsync(int id, CancellationToken ct)
    {
        return await dbContext.Loans
            .Include(x => x.Book)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
    }

    public async Task UpdateLoanAsync(LoanEntity loan, CancellationToken ct)
    {
        loan.UpdatedDate = DateTime.UtcNow;
        dbContext.Loans.Update(loan);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Busca si hay un préstamo ACTIVO (sin devolver) para un libro
    /// Usado para validar: "¿Este libro ya está prestado?"
    /// </summary>
    public async Task<LoanEntity?> GetActiveLoanByBookIdAsync(int bookId, CancellationToken ct)
    {
        return await dbContext.Loans
            .FirstOrDefaultAsync(x => x.BookId == bookId && x.ReturnDate == null, cancellationToken: ct);
    }
}
