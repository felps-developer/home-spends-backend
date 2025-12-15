using HomeSpends.Application.DTOs.Category;
using HomeSpends.Application.DTOs.Person;
using HomeSpends.Application.DTOs.Transaction;
using HomeSpends.Domain.Entities;

namespace HomeSpends.Application.Services;

/// <summary>
/// Serviço de mapeamento centralizado entre entidades e DTOs.
/// Elimina duplicação de código de mapeamento seguindo o princípio DRY.
/// </summary>
public class MapperService : IMapperService
{
    /// <summary>
    /// Converte uma entidade Category para seu DTO correspondente.
    /// </summary>
    /// <param name="category">Entidade Category a ser convertida.</param>
    /// <returns>DTO da categoria com todos os campos mapeados.</returns>
    public CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Description = category.Description,
            Purpose = category.Purpose,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    /// <summary>
    /// Converte uma entidade Person para seu DTO correspondente.
    /// </summary>
    /// <param name="person">Entidade Person a ser convertida.</param>
    /// <returns>DTO da pessoa com todos os campos mapeados.</returns>
    public PersonDto MapToDto(Person person)
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

    /// <summary>
    /// Converte uma entidade Transaction para seu DTO correspondente.
    /// IMPORTANTE: Este método assume que os relacionamentos (Category e Person) já foram carregados.
    /// Use GetByIdWithDetailsAsync ou GetAllWithDetailsAsync no repositório antes de chamar este método.
    /// </summary>
    /// <param name="transaction">Entidade Transaction a ser convertida (deve ter Category e Person carregados).</param>
    /// <returns>DTO da transação com todos os campos e relacionamentos mapeados.</returns>
    public TransactionDto MapToDto(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Value = transaction.Value,
            Type = transaction.Type,
            // Mapeia o relacionamento Category para CategoryDto
            Category = new CategoryDto
            {
                Id = transaction.Category.Id,
                Description = transaction.Category.Description,
                Purpose = transaction.Category.Purpose,
                CreatedAt = transaction.Category.CreatedAt,
                UpdatedAt = transaction.Category.UpdatedAt
            },
            // Mapeia o relacionamento Person para PersonDto
            Person = new PersonDto
            {
                Id = transaction.Person.Id,
                Name = transaction.Person.Name,
                Age = transaction.Person.Age,
                CreatedAt = transaction.Person.CreatedAt,
                UpdatedAt = transaction.Person.UpdatedAt
            },
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt
        };
    }
}

