using Application.DTOs.Books;
using Core.Domains.Books;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    /// <summary>
    /// Crear un nuevo libro con portada (multipart/form-data)
    /// </summary>
    /// <remarks>
    /// Enviar:
    /// - title: string
    /// - numberOfPages: int
    /// - authorId: int (debe existir)
    /// - genre: string
    /// - publishedDate: DateTime
    /// - isbn: string (opcional)
    /// - file: IFormFile (jpg, png, webp - opcional)
    /// </remarks>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> CreateBook(
        [FromForm] string title,
        [FromForm] int numberOfPages,
        [FromForm] int authorId,
        [FromForm] string genre,
        [FromForm] DateTime publishedDate,
        [FromForm] string? isbn,
        [FromForm] IFormFile? file,
        [FromServices] CreateBookDomain createBookDomain,
        [FromServices] IImageService imageService,
        CancellationToken ct)
    {
        try
        {
            Console.WriteLine($"[BookController.CreateBook] Called");
            Console.WriteLine($"[BookController.CreateBook] Title: {title}, Pages: {numberOfPages}, AuthorId: {authorId}");
            Console.WriteLine($"[BookController.CreateBook] File received: {file?.FileName ?? "null"}, Size: {file?.Length ?? 0}");

            // Guardar imagen si existe
            string? coverImagePath = null;
            if (file != null)
            {
                Console.WriteLine($"[BookController.CreateBook] Processing image upload");
                coverImagePath = await imageService.SaveImageAsync(file, "uploads/books");
                Console.WriteLine($"[BookController.CreateBook] Image saved at: {coverImagePath}");
            }
            else
            {
                Console.WriteLine($"[BookController.CreateBook] No file provided");
            }

            var request = new CreateBookRequest
            {
                Title = title,
                NumberOfPages = numberOfPages,
                AuthorId = authorId,
                Genre = genre,
                PublishedDate = publishedDate,
                ISBN = isbn
            };

            var result = await createBookDomain.CreateAsync(request, coverImagePath, ct);
            Console.WriteLine($"[BookController.CreateBook] Book created successfully with ID: {result.Id}");
            return CreatedAtAction(nameof(GetBookById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"[BookController.CreateBook] KeyNotFoundException: {ex.Message}");
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[BookController.CreateBook] ArgumentException: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (BadImageFormatException ex)
        {
            Console.WriteLine($"[BookController.CreateBook] BadImageFormatException: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BookController.CreateBook] Exception: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(500, new { message = "Error al crear libro", error = ex.Message });
        }
    }

    /// <summary>
    /// Listar libros con paginación y filtros
    /// </summary>
    /// <remarks>
    /// Parámetros:
    /// - page: número de página (1 por defecto)
    /// - pageSize: cantidad por página (10 por defecto)
    /// - authorId: filtrar por autor (opcional)
    /// - title: buscar por título (opcional)
    /// - sortBy: ordenamiento (ej: "b.Title ASC")
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetBooks(
        [FromServices] GetBooksDomain getBooksDomain,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] int? authorId,
        [FromQuery] string? title,
        [FromQuery] string? sortBy,
        CancellationToken ct)
    {
        try
        {
            var pageNum = page ?? 1;
            var pageSizeNum = pageSize ?? 10;
            var sortByStr = sortBy ?? "b.CreatedDate DESC";

            var result = await getBooksDomain.GetBooksAsync(pageNum, pageSizeNum, authorId, title, sortByStr, ct);
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
    /// Obtener detalles de un libro por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(
        [FromRoute] int id,
        [FromServices] GetBookDetailDomain getBookDetailDomain,
        CancellationToken ct)
    {
        try
        {
            var result = await getBookDetailDomain.GetByIdAsync(id, ct);
            if (result == null)
                return NotFound(new { message = $"Libro con ID {id} no encontrado" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Actualizar un libro
    /// </summary>
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> UpdateBook(
        [FromRoute] int id,
        [FromForm] string title,
        [FromForm] int numberOfPages,
        [FromForm] string genre,
        [FromForm] DateTime publishedDate,
        [FromForm] string? isbn,
        [FromForm] IFormFile? file,
        [FromServices] UpdateBookDomain updateBookDomain,
        [FromServices] IImageService imageService,
        CancellationToken ct)
    {
        try
        {
            string? newCoverImagePath = null;
            if (file != null)
            {
                newCoverImagePath = await imageService.SaveImageAsync(file, "uploads/books");
            }

            var request = new UpdateBookRequest
            {
                Title = title,
                NumberOfPages = numberOfPages,
                Genre = genre,
                PublishedDate = publishedDate,
                ISBN = isbn
            };

            var result = await updateBookDomain.UpdateAsync(id, request, newCoverImagePath, ct);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Eliminar un libro (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(
        [FromRoute] int id,
        [FromServices] DeleteBookDomain deleteBookDomain,
        CancellationToken ct)
    {
        try
        {
            await deleteBookDomain.DeleteAsync(id, ct);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
