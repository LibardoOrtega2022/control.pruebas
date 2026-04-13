using Application.Abstractions.Reports;
using Application.DTOs.Reports;
using Dapper;
using Infrastructure.Persistences;

namespace Infrastructure.Queries.Reports;

public class ReportQueries(UnitOfWork unitOfWork) : IReportQueries
{
    /// <summary>
    /// TOP 5 AUTORES POR TOTAL DE PÁGINAS
    /// 
    /// ¿Por qué es eficiente?
    /// - Suma las páginas EN LA BD (no en memoria)
    /// - Usa índice (AuthorId, IsDeleted) en Books
    /// - Ordena en BD (no aquí)
    /// - Solo trae 5 resultados (LIMIT 5)
    /// 
    /// SQL: SELECT a.Id, a.Name+LastName, SUM(b.NumberOfPages), COUNT(*)
    ///      FROM Author a
    ///      JOIN Books b (con índice)
    ///      WHERE a.IsDeleted=0 AND b.IsDeleted=0
    ///      GROUP BY a.Id (agrupa por autor)
    ///      ORDER BY SUM(pages) DESC
    ///      LIMIT 5
    /// </summary>
    public async Task<List<TopAuthorsReportDto>> GetTop5AuthorsByPagesAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT TOP 5
                a.Id as AuthorId,
                CONCAT(a.Name, ' ', a.LastName) as AuthorName,
                COALESCE(SUM(b.NumberOfPages), 0) as TotalPages,
                COUNT(b.Id) as BookCount
            FROM Author a
            LEFT JOIN Books b ON a.Id = b.AuthorId AND b.IsDeleted = 0
            WHERE a.IsDeleted = 0
            GROUP BY a.Id, a.Name, a.LastName
            ORDER BY COALESCE(SUM(b.NumberOfPages), 0) DESC";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            var result = await conn.QueryAsync<TopAuthorsReportDto>(sql);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetTop5AuthorsByPagesAsync: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// AUTORES SIN LIBROS
    /// 
    /// ¿Por qué es eficiente?
    /// - LEFT JOIN: trae autores aunque no tengan libros
    /// - WHERE b.Id IS NULL: filtra autores sin libros
    /// - Índice en Books (AuthorId,IsDeleted) hace el JOIN rápido
    /// </summary>
    public async Task<List<AuthorWithoutBooksDto>> GetAuthorsWithoutBooksAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT 
                a.Id as AuthorId,
                CONCAT(a.Name, ' ', a.LastName) as AuthorName,
                a.Country
            FROM Author a
            LEFT JOIN Books b ON a.Id = b.AuthorId AND b.IsDeleted = 0
            WHERE a.IsDeleted = 0 AND b.Id IS NULL";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            var result = await conn.QueryAsync<AuthorWithoutBooksDto>(sql);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetAuthorsWithoutBooksAsync: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// PROMEDIO DE PÁGINAS POR LIBRO
    /// 
    /// ¿Por qué es eficiente?
    /// - AVG() lo calcula en BD (una sola operación)
    /// - COUNT() lo hace en BD
    /// - Resultado: 2 números, no 10.000 libros en memoria
    /// </summary>
    public async Task<AveragePagesByBookDto> GetAveragePagesPerBookAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT 
                COALESCE(CAST(AVG(CAST(NumberOfPages as DECIMAL(10,2))) AS DECIMAL(10,2)), 0) as AveragePages,
                COUNT(*) as TotalBooks
            FROM Books
            WHERE IsDeleted = 0";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            var result = await conn.QueryFirstOrDefaultAsync<AveragePagesByBookDto>(sql);
            return result ?? new AveragePagesByBookDto { AveragePages = 0, TotalBooks = 0 };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetAveragePagesPerBookAsync: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// TOTAL DE LIBROS POR AUTOR (ordenado descendente)
    /// 
    /// ¿Por qué es eficiente?
    /// - GROUP BY en BD (agrupa rápido)
    /// - COUNT(*): contar libros por autor en BD
    /// - ORDER BY COUNT DESC: ordena por cantidad en BD
    /// </summary>
    public async Task<List<TotalBooksByAuthorDto>> GetTotalBooksByAuthorAsync(CancellationToken ct)
    {
        const string sql = @"
            SELECT 
                a.Id as AuthorId,
                CONCAT(a.Name, ' ', a.LastName) as AuthorName,
                COUNT(b.Id) as BookCount
            FROM Author a
            LEFT JOIN Books b ON a.Id = b.AuthorId AND b.IsDeleted = 0
            WHERE a.IsDeleted = 0
            GROUP BY a.Id, a.Name, a.LastName
            ORDER BY COUNT(b.Id) DESC";

        try
        {
            var conn = unitOfWork.Connection;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            var result = await conn.QueryAsync<TotalBooksByAuthorDto>(sql);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en GetTotalBooksByAuthorAsync: {ex.Message}", ex);
        }
    }
}
