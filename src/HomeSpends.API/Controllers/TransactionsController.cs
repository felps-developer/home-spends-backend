using HomeSpends.Application.DTOs.Transaction;
using HomeSpends.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeSpends.API.Controllers;

/// <summary>
/// Controller para gerenciamento de transações.
/// </summary>
public class TransactionsController : BaseController
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
        : base(logger)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Lista todas as transações cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var transactions = await _transactionService.GetAllAsync(cancellationToken);
        return Ok(transactions);
    }

    /// <summary>
    /// Busca uma transação pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _transactionService.GetByIdAsync(id, cancellationToken);
        if (transaction == null)
        {
            return NotFound("Transação", id);
        }
        return Ok(transaction);
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
        var validationResult = ValidateModelState();
        if (validationResult != null)
        {
            return validationResult;
        }

        var transaction = await _transactionService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }
}

