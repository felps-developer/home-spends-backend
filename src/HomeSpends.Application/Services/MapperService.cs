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

    public TransactionDto MapToDto(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Value = transaction.Value,
            Type = transaction.Type,
            Category = new CategoryDto
            {
                Id = transaction.Category.Id,
                Description = transaction.Category.Description,
                Purpose = transaction.Category.Purpose,
                CreatedAt = transaction.Category.CreatedAt,
                UpdatedAt = transaction.Category.UpdatedAt
            },
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

