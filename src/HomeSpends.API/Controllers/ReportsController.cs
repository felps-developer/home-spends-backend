using HomeSpends.Application.DTOs.Reports;
using HomeSpends.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeSpends.API.Controllers;

/// <summary>
/// Controller para relatórios de totais.
/// </summary>
public class ReportsController : BaseController
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        : base(logger)
    {
        _reportService = reportService;
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
        var report = await _reportService.GetPersonTotalsAsync(cancellationToken);
        return Ok(report);
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
        var report = await _reportService.GetCategoryTotalsAsync(cancellationToken);
        return Ok(report);
    }
}

