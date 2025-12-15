using HomeSpends.Application.DTOs.Person;
using HomeSpends.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeSpends.API.Controllers;

/// <summary>
/// Controller para gerenciamento de pessoas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PeopleController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(IPersonService personService, ILogger<PeopleController> logger)
    {
        _personService = personService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as pessoas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PersonDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var people = await _personService.GetAllAsync(cancellationToken);
            return Ok(people);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pessoas");
            return StatusCode(500, new { message = "Erro interno do servidor ao listar pessoas" });
        }
    }

    /// <summary>
    /// Busca uma pessoa pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var person = await _personService.GetByIdAsync(id, cancellationToken);
            if (person == null)
            {
                return NotFound(new { message = $"Pessoa com ID {id} não encontrada" });
            }
            return Ok(person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoa com ID: {PersonId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor ao buscar pessoa" });
        }
    }

    /// <summary>
    /// Cria uma nova pessoa.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonDto>> Create([FromBody] CreatePersonDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = await _personService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa");
            return StatusCode(500, new { message = "Erro interno do servidor ao criar pessoa" });
        }
    }

    /// <summary>
    /// Deleta uma pessoa pelo ID.
    /// Ao deletar uma pessoa, todas as transações dessa pessoa são apagadas automaticamente.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _personService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Pessoa com ID {id} não encontrada" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar pessoa com ID: {PersonId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor ao deletar pessoa" });
        }
    }
}

