using Application.Abstractions.Loans;
using Application.DTOs.Loans;
using Dapper;
using Infrastructure.Persistences;

namespace Infrastructure.Queries.Loans;

public class LoanQueries(UnitOfWork unitOfWork) : ILoanQueries
{
    public async Task<List<LoanResponse>> GetLoansAsync(int page, int pageSize, string? status, int? bookId, CancellationToken ct)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var offset = (page - 1) * pageSize;

        var sql = @"
            SELECT 
                l.Id,
                l.BookId,
                b.Title as BookTitle,
                l.BorrowerName,
                l.LoanDate,
                l.DueDate,
                l.ReturnDate,
                l.Status,
                l.CreatedDate
            FROM Loans l
            INNER JOIN Books b ON l.BookId = b.Id
            WHERE 1=1";

        if (!string.IsNullOrWhiteSpace(status))
            sql += $" AND l.Status = '{status}'";

        if (bookId.HasValue)
            sql += $" AND l.BookId = {bookId}";

        sql += $@"
            ORDER BY l.CreatedDate DESC
            OFFSET {offset} ROWS
            FETCH NEXT {pageSize} ROWS ONLY";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            
            var result = await conn.QueryAsync<LoanResponse>(sql);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetLoansAsync: {ex.Message}", ex);
        }
    }

    public async Task<LoanResponse?> GetLoanByIdAsync(int id, CancellationToken ct)
    {
        const string sql = @"
            SELECT 
                l.Id,
                l.BookId,
                b.Title as BookTitle,
                l.BorrowerName,
                l.LoanDate,
                l.DueDate,
                l.ReturnDate,
                l.Status,
                l.CreatedDate
            FROM Loans l
            INNER JOIN Books b ON l.BookId = b.Id
            WHERE l.Id = @Id";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            
            var result = await conn.QueryFirstOrDefaultAsync<LoanResponse>(sql, new { Id = id });
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetLoanByIdAsync: {ex.Message}", ex);
        }
    }
}
