using HomeSpends.Application.DTOs.Transaction;
using HomeSpends.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeSpends.API.Controllers;

/// <summary>
/// Controller para gerenciamento de transações.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as transações cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var transactions = await _transactionService.GetAllAsync(cancellationToken);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar transações");
            return StatusCode(500, new { message = "Erro interno do servidor ao listar transações" });
        }
    }

    /// <summary>
    /// Busca uma transação pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _transactionService.GetByIdAsync(id, cancellationToken);
            if (transaction == null)
            {
                return NotFound(new { message = $"Transação com ID {id} não encontrada" });
            }
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar transação com ID: {TransactionId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor ao buscar transação" });
        }
    }

    /// <summary>
    /// Cria uma nova transação.
    /// Validações aplicadas:
    /// - Menores de idade (menor de 18 anos) só podem ter despesas
    /// - A categoria deve permitir o tipo de transação (despesa/receita)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TransactionDto>> Create([FromBody] CreateTransactionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _transactionService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar transação");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar transação");
            return StatusCode(500, new { message = "Erro interno do servidor ao criar transação" });
        }
    }
}

