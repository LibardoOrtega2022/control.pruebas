using Application.DTOs.Authors;
using Core.Domains.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    /// <summary>
    /// Crear un nuevo autor
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAuthor(
        [FromBody] CreateAuthorRequest request,
        [FromServices] CreateAuthorDomain createAuthorDomain,
        CancellationToken ct)
    {
        try
        {
            var result = await createAuthorDomain.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetAuthorById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Listar autores con paginación y ordenamiento
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAuthors(
        [FromServices] AuthorListDomain authorListDomain,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? sortBy,
        CancellationToken ct)
    {
        try
        {
            var pageNum = page ?? 1;
            var pageSizeNum = pageSize ?? 10;
            var sortByStr = sortBy ?? "CreatedDate DESC";

            var result = await authorListDomain.GetAuthorsAsync(pageNum, pageSizeNum, sortByStr, ct);
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
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtener autor por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthorById(
        [FromRoute] int id,
        [FromServices] GetAuthorDomain getAuthorDomain,
        CancellationToken ct)
    {
        try
        {
            var result = await getAuthorDomain.GetByIdAsync(id, ct);
            if (result == null)
                return NotFound(new { message = $"Autor con ID {id} no encontrado" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualizar un autor
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(
        [FromRoute] int id,
        [FromBody] UpdateAuthorRequest request,
        [FromServices] UpdateAuthorDomain updateAuthorDomain,
        CancellationToken ct)
    {
        try
        {
            var result = await updateAuthorDomain.UpdateAsync(id, request, ct);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Eliminar un autor (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(
        [FromRoute] int id,
        [FromServices] DeleteAuthorDomain deleteAuthorDomain,
        CancellationToken ct)
    {
        try
        {
            await deleteAuthorDomain.DeleteAsync(id, ct);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}