namespace HomeSpends.Application.DTOs.Person;

/// <summary>
/// DTO para representação de uma pessoa.
/// </summary>
public class PersonDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

