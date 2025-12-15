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
    private readonly IMapperService _mapper;
    private readonly ILogger<PersonService> _logger;

    public PersonService(
        IPersonRepository repository,
        IMapperService mapper,
        ILogger<PersonService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Busca todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de todas as pessoas convertidas para DTO.</returns>
    public async Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando todas as pessoas");
            // Busca todas as pessoas do repositório
            var people = await _repository.GetAllAsync(cancellationToken);
            // Converte entidades para DTOs usando o serviço de mapeamento
            return people.Select(_mapper.MapToDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoas");
            throw;
        }
    }

    /// <summary>
    /// Busca uma pessoa específica pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador único da pessoa.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO da pessoa encontrada ou null se não existir.</returns>
    public async Task<PersonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Buscando pessoa com ID: {PersonId}", id);
            // Busca pessoa pelo ID
            var person = await _repository.GetByIdAsync(id, cancellationToken);
            // Retorna null se não encontrada, caso contrário converte para DTO
            return person != null ? _mapper.MapToDto(person) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoa com ID: {PersonId}", id);
            throw;
        }
    }

    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// A idade da pessoa é importante para aplicar regras de negócio:
    /// - Pessoas menores de 18 anos (IsMinor = true) só podem ter despesas, não receitas.
    /// </summary>
    /// <param name="dto">DTO com os dados da pessoa a ser criada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO da pessoa criada.</returns>
    public async Task<PersonDto> CreateAsync(CreatePersonDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando nova pessoa: {Name}", dto.Name);

            // Cria a entidade Person com os dados do DTO
            var person = new Person
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Age = dto.Age, // A idade determina se a pessoa é menor (IsMinor = Age < 18)
                CreatedAt = DateTime.UtcNow
            };

            // Persiste a pessoa no banco de dados
            var created = await _repository.AddAsync(person, cancellationToken);
            _logger.LogInformation("Pessoa criada com sucesso: {PersonId}", created.Id);

            // Converte a entidade para DTO e retorna
            return _mapper.MapToDto(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa");
            throw;
        }
    }

    /// <summary>
    /// Deleta uma pessoa do sistema.
    /// IMPORTANTE: Ao deletar uma pessoa, todas as transações associadas são deletadas automaticamente
    /// devido ao DeleteBehavior.Cascade configurado no ApplicationDbContext.
    /// Esta é uma operação irreversível.
    /// </summary>
    /// <param name="id">Identificador único da pessoa a ser deletada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <exception cref="KeyNotFoundException">Lançada quando a pessoa não é encontrada.</exception>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deletando pessoa com ID: {PersonId}", id);

            // Verificar se a pessoa existe antes de tentar deletar
            var person = await _repository.GetByIdAsync(id, cancellationToken);
            if (person == null)
            {
                throw new KeyNotFoundException($"Pessoa com ID {id} não encontrada");
            }

            // Ao deletar uma pessoa, todas as transações são deletadas automaticamente
            // devido ao DeleteBehavior.Cascade configurado no DbContext (OnDelete(DeleteBehavior.Cascade))
            // Isso garante integridade referencial e evita transações órfãs
            await _repository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Pessoa deletada com sucesso: {PersonId}", id);
        }
        catch (KeyNotFoundException)
        {
            // Re-lança KeyNotFoundException para que o controller possa retornar 404
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar pessoa com ID: {PersonId}", id);
            throw;
        }
    }
}

