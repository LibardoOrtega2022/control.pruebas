using Application.Abstractions.Authors;
using Application.DTOs.Authors;
using Dapper;
using Infrastructure.Persistences;

namespace Infrastructure.Queries.Authors;

public class AuthorQueries(UnitOfWork unitOfWork) : IAuthorQueries
{
    public async Task<List<AuthorResponse>> GetAuthorsAsync(int page, int pageSize, string? sortBy, CancellationToken ct)
    {
        // Validar entrada
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        sortBy = string.IsNullOrWhiteSpace(sortBy) ? "CreatedDate DESC" : sortBy;

        var offset = (page - 1) * pageSize;

        // SQL con paginación
        var sql = $@"
            SELECT 
                a.Id,
                a.Name,
                a.LastName,
                a.BirthDate,
                a.Country,
                a.Biography,
                a.CreatedDate,
                a.UpdatedDate,
                (SELECT COUNT(*) FROM Books WHERE AuthorId = a.Id) as BookCount
            FROM Author a
            WHERE a.IsDeleted = 0
            ORDER BY {sortBy}
            OFFSET {offset} ROWS
            FETCH NEXT {pageSize} ROWS ONLY";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            
            var result = await conn.QueryAsync<AuthorResponse>(sql);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetAuthorsAsync: {ex.Message}", ex);
        }
    }

    public async Task<AuthorResponse?> GetAuthorByIdAsync(int id, CancellationToken ct)
    {
        const string sql = @"
            SELECT 
                a.Id,
                a.Name,
                a.LastName,
                a.BirthDate,
                a.Country,
                a.Biography,
                a.CreatedDate,
                a.UpdatedDate,
                (SELECT COUNT(*) FROM Books WHERE AuthorId = a.Id) as BookCount
            FROM Author a
            WHERE a.Id = @Id AND a.IsDeleted = 0";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            
            var result = await conn.QueryFirstOrDefaultAsync<AuthorResponse>(sql, new { Id = id });
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetAuthorByIdAsync: {ex.Message}", ex);
        }
    }
}