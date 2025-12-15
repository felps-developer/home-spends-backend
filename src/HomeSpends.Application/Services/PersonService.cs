using HomeSpends.Application.DTOs.Person;
using HomeSpends.Domain.Entities;
using HomeSpends.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeSpends.Application.Services;

/// <summary>
/// Serviço de aplicação para pessoas.
/// Implementa as regras de negócio relacionadas a pessoas.
/// </summary>
public class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;
    private readonly ILogger<PersonService> _logger;

    public PersonService(IPersonRepository repository, ILogger<PersonService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando todas as pessoas");
            var people = await _repository.GetAllAsync(cancellationToken);
            return people.Select(MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoas");
            throw;
        }
    }

    public async Task<PersonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando pessoa com ID: {PersonId}", id);
            var person = await _repository.GetByIdAsync(id, cancellationToken);
            return person != null ? MapToDto(person) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoa com ID: {PersonId}", id);
            throw;
        }
    }

    public async Task<PersonDto> CreateAsync(CreatePersonDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando nova pessoa: {Name}", dto.Name);

            var person = new Person
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Age = dto.Age,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(person, cancellationToken);
            _logger.LogInformation("Pessoa criada com sucesso: {PersonId}", created.Id);

            return MapToDto(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa");
            throw;
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deletando pessoa com ID: {PersonId}", id);

            // Verificar se a pessoa existe
            var person = await _repository.GetByIdAsync(id, cancellationToken);
            if (person == null)
            {
                throw new KeyNotFoundException($"Pessoa com ID {id} não encontrada");
            }

            // Ao deletar uma pessoa, todas as transações são deletadas automaticamente
            // devido ao DeleteBehavior.Cascade configurado no DbContext
            await _repository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Pessoa deletada com sucesso: {PersonId}", id);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar pessoa com ID: {PersonId}", id);
            throw;
        }
    }

    private static PersonDto MapToDto(Person person)
    {
        return new PersonDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age,
            CreatedAt = person.CreatedAt,
            UpdatedAt = person.UpdatedAt
        };
    }
}

