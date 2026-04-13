using Core.Domains.Reports;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    /// <summary>
    /// Obtener resumen completo de la librería
    /// </summary>
    /// <remarks>
    /// Retorna un objeto con:
    /// - Top 5 autores por total de páginas (ranking)
    /// - Lista de autores sin libros
    /// - Promedio de páginas por libro
    /// - Total de libros por autor (ordenado por cantidad desc)
    /// 
    /// NOTA: Todo esto se calcula eficientemente en la base de datos.
    /// No cargamos 100.000 libros en memoria, solo los totales.
    /// 
    /// Tiempo esperado: < 500ms incluso con millones de registros
    /// </remarks>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromServices] GenerateLibrarySummaryReportDomain reportDomain,
        CancellationToken ct)
    {
        try
        {
            var result = await reportDomain.GetSummaryAsync(ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al generar reporte", error = ex.Message });
        }
    }
}

