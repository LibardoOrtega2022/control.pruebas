using Application.Abstractions;
using Application.Abstractions.Books;
using Application.Abstractions.Loans;
using Application.DTOs.Loans;
using Application.Entities;

namespace Core.Domains.Loans;

public class CreateLoanDomain(
    ILoanRepository loanRepo,
    IBookRepository bookRepo,
    IUnitOfWork uow)
{
    public async Task<LoanResponse> CreateAsync(CreateLoanRequest request, CancellationToken ct)
    {
        // Validar que el libro exista
        var book = await bookRepo.GetBookByIdAsync(request.BookId, ct);
        if (book == null)
            throw new KeyNotFoundException($"Libro con ID {request.BookId} no encontrado");

        // ⭐ VALIDACIÓN CRÍTICA: Un libro NO puede tener 2 préstamos activos
        var activeLoan = await loanRepo.GetActiveLoanByBookIdAsync(request.BookId, ct);
        if (activeLoan != null)
            throw new InvalidOperationException($"El libro '{book.Title}' ya tiene un préstamo activo. No puede prestarse dos veces.");

        // Validar fechas
        if (request.LoanDate > request.DueDate)
            throw new ArgumentException("La fecha de préstamo no puede ser mayor a la fecha de devolución");

        var loan = new LoanEntity
        {
            BookId = request.BookId,
            BorrowerName = request.BorrowerName,
            LoanDate = request.LoanDate,
            DueDate = request.DueDate,
            ReturnDate = null,
            Status = "Active",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = null
        };

        await uow.BeginAsync(ct);

        try
        {
            await loanRepo.AddLoanAsync(loan, ct);
            await uow.CommitAsync(ct);

            return new LoanResponse
            {
                Id = loan.Id,
                BookId = loan.BookId,
                BookTitle = book.Title,
                BorrowerName = loan.BorrowerName,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Status = loan.Status,
                CreatedDate = loan.CreatedDate
            };
        }
        catch
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
