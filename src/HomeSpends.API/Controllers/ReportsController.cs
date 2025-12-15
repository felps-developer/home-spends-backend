using HomeSpends.Application.DTOs.Reports;
using HomeSpends.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeSpends.API.Controllers;

/// <summary>
/// Controller para relatórios de totais.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Consulta de totais por pessoa.
    /// Lista todas as pessoas cadastradas, exibindo o total de receitas, despesas e o saldo de cada uma.
    /// Ao final, exibe o total geral de todas as pessoas.
    /// </summary>
    [HttpGet("person-totals")]
    [ProducesResponseType(typeof(PersonTotalsReportDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PersonTotalsReportDto>> GetPersonTotals(CancellationToken cancellationToken)
    {
        try
        {
            var report = await _reportService.GetPersonTotalsAsync(cancellationToken);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de totais por pessoa");
            return StatusCode(500, new { message = "Erro interno do servidor ao gerar relatório" });
        }
    }

    /// <summary>
    /// Consulta de totais por categoria (opcional).
    /// Lista todas as categorias cadastradas, exibindo o total de receitas, despesas e o saldo de cada uma.
    /// Ao final, exibe o total geral de todas as categorias.
    /// </summary>
    [HttpGet("category-totals")]
    [ProducesResponseType(typeof(CategoryTotalsReportDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryTotalsReportDto>> GetCategoryTotals(CancellationToken cancellationToken)
    {
        try
        {
            var report = await _reportService.GetCategoryTotalsAsync(cancellationToken);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de totais por categoria");
            return StatusCode(500, new { message = "Erro interno do servidor ao gerar relatório" });
        }
    }
}

