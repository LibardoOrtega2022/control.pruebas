using Application.Abstractions.Books;
using Application.DTOs.Books;
using Dapper;
using Infrastructure.Persistences;

namespace Infrastructure.Queries.Books;

public class BookQueries(UnitOfWork unitOfWork) : IBookQueries
{
    public async Task<List<BookResponse>> GetBooksAsync(
        int page, 
        int pageSize, 
        int? authorId, 
        string? title, 
        string? sortBy, 
        CancellationToken ct)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        sortBy = string.IsNullOrWhiteSpace(sortBy) ? "b.CreatedDate DESC" : sortBy;

        var offset = (page - 1) * pageSize;

        // Construir SQL dinámicamente
        var sql = $@"
            SELECT 
                b.Id,
                b.Title,
                b.NumberOfPages,
                b.AuthorId,
                CONCAT(a.Name, ' ', a.LastName) as AuthorName,
                b.Genre,
                b.PublishedDate,
                b.ISBN,
                b.CoverImagePath,
                b.CreatedDate,
                b.UpdatedDate
            FROM Books b
            INNER JOIN Author a ON b.AuthorId = a.Id
            WHERE b.IsDeleted = 0";

        // Agregar filtros dinámicamente
        if (authorId.HasValue)
            sql += $" AND b.AuthorId = {authorId}";

        if (!string.IsNullOrWhiteSpace(title))
            sql += $" AND b.Title LIKE '%{title}%'";

        sql += $@"
            ORDER BY {sortBy}
            OFFSET {offset} ROWS
            FETCH NEXT {pageSize} ROWS ONLY";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            
            var result = await conn.QueryAsync<BookResponse>(sql);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetBooksAsync: {ex.Message}", ex);
        }
    }

    public async Task<BookResponse?> GetBookByIdAsync(int id, CancellationToken ct)
    {
        const string sql = @"
            SELECT 
                b.Id,
                b.Title,
                b.NumberOfPages,
                b.AuthorId,
                CONCAT(a.Name, ' ', a.LastName) as AuthorName,
                b.Genre,
                b.PublishedDate,
                b.ISBN,
                b.CoverImagePath,
                b.CreatedDate,
                b.UpdatedDate
            FROM Books b
            INNER JOIN Author a ON b.AuthorId = a.Id
            WHERE b.Id = @Id AND b.IsDeleted = 0";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            
            var result = await conn.QueryFirstOrDefaultAsync<BookResponse>(sql, new { Id = id });
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetBookByIdAsync: {ex.Message}", ex);
        }
    }
}
