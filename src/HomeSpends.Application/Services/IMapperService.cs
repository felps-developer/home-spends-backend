using HomeSpends.Application.DTOs.Category;
using HomeSpends.Application.DTOs.Person;
using HomeSpends.Application.DTOs.Transaction;
using HomeSpends.Domain.Entities;

namespace HomeSpends.Application.Services;

/// <summary>
/// Interface para serviço de mapeamento entre entidades e DTOs.
/// Segue o princípio Single Responsibility e Dependency Inversion.
/// </summary>
public interface IMapperService
{
    CategoryDto MapToDto(Category category);
    PersonDto MapToDto(Person person);
    TransactionDto MapToDto(Transaction transaction);
}

