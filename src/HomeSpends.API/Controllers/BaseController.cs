using Microsoft.AspNetCore.Mvc;

namespace HomeSpends.API.Controllers;

/// <summary>
/// Controller base que fornece métodos auxiliares comuns para todos os controllers.
/// Segue o princípio DRY (Don't Repeat Yourself) e Single Responsibility.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    protected readonly ILogger _logger;

    protected BaseController(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retorna uma resposta 404 Not Found padronizada.
    /// </summary>
    protected ActionResult NotFound(string entityName, Guid id)
    {
        return NotFound(new { message = $"{entityName} com ID {id} não encontrada" });
    }

    /// <summary>
    /// Retorna uma resposta 500 Internal Server Error padronizada.
    /// </summary>
    protected ActionResult InternalServerError(string operation, Exception ex)
    {
        _logger.LogError(ex, "Erro ao {Operation}", operation);
        return StatusCode(500, new { message = $"Erro interno do servidor ao {operation}" });
    }

    /// <summary>
    /// Valida o ModelState e retorna BadRequest se inválido.
    /// </summary>
    protected ActionResult? ValidateModelState()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return null;
    }
}

