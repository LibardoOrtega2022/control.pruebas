using Application.DTOs.Loans;
using Core.Domains.Loans;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    /// <summary>
    /// Crear un nuevo préstamo de libro
    /// </summary>
    /// <remarks>
    /// Regla: Un libro solo puede tener UN préstamo activo (ReturnDate == null).
    /// Si intenta prestar un libro que ya está prestado, recibirá 409 Conflict.
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> CreateLoan(
        [FromBody] CreateLoanRequest request,
        [FromServices] CreateLoanDomain createLoanDomain,
        CancellationToken ct)
    {
        try
        {
            var result = await createLoanDomain.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetLoanById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Cuando un libro ya tiene préstamo activo
            return StatusCode(409, new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear préstamo", error = ex.Message });
        }
    }

    /// <summary>
    /// Listar préstamos con filtros
    /// </summary>
    /// <remarks>
    /// Parámetros:
    /// - page: número de página
    /// - pageSize: cantidad por página
    /// - status: filtrar por estado (Active, Returned, Overdue)
    /// - bookId: filtrar por libro
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetLoans(
        [FromServices] GetLoansDomain getLoansDomain,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? status,
        [FromQuery] int? bookId,
        CancellationToken ct)
    {
        try
        {
            var pageNum = page ?? 1;
            var pageSizeNum = pageSize ?? 10;

            var result = await getLoansDomain.GetLoansAsync(pageNum, pageSizeNum, status, bookId, ct);
            return Ok(new
            {
                page = pageNum,
                pageSize = pageSizeNum,
                data = result,
                count = result.Count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtener detalles de un préstamo
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanById(
        [FromRoute] int id,
        [FromServices] GetLoanDetailDomain getLoanDetailDomain,
        CancellationToken ct)
    {
        try
        {
            var result = await getLoanDetailDomain.GetByIdAsync(id, ct);
            if (result == null)
                return NotFound(new { message = $"Préstamo con ID {id} no encontrado" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Marcar un préstamo como devuelto
    /// </summary>
    /// <remarks>
    /// Cuando se devuelve después de la fecha de vencimiento,
    /// el estado cambia automáticamente a "Overdue"
    /// </remarks>
    [HttpPut("{id}/return")]
    public async Task<IActionResult> ReturnBook(
        [FromRoute] int id,
        [FromServices] ReturnLoanDomain returnLoanDomain,
        CancellationToken ct)
    {
        try
        {
            var result = await returnLoanDomain.ReturnBookAsync(id, ct);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
