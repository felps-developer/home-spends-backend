using HomeSpends.Application.DTOs.Person;

namespace HomeSpends.Application.Services;

/// <summary>
/// Interface para o serviço de pessoas.
/// Define operações de negócio relacionadas a pessoas.
/// </summary>
public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PersonDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PersonDto> CreateAsync(CreatePersonDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

