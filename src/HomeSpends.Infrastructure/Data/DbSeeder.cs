using HomeSpends.Domain.Entities;

namespace HomeSpends.Infrastructure.Data;

/// <summary>
/// Classe responsável por popular o banco de dados com dados iniciais (seed).
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Popula o banco de dados com dados iniciais.
    /// Cria 5 pessoas, 5 categorias e 5 transações.
    /// </summary>
    public static void Seed(ApplicationDbContext context)
    {
        // Verifica se já existem dados no banco
        if (context.People.Any() || context.Categories.Any() || context.Transactions.Any())
        {
            return; // Já existem dados, não popula novamente
        }

        var now = DateTime.UtcNow;

        // Criar 5 pessoas
        var people = new List<Person>
        {
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "João Silva",
                Age = 35,
                CreatedAt = now
            },
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "Maria Santos",
                Age = 28,
                CreatedAt = now
            },
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "Pedro Oliveira",
                Age = 42,
                CreatedAt = now
            },
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "Ana Costa",
                Age = 16, // Menor de idade para testar validação
                CreatedAt = now
            },
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "Carlos Ferreira",
                Age = 50,
                CreatedAt = now
            }
        };

        context.People.AddRange(people);
        context.SaveChanges();

        // Criar 5 categorias
        var categories = new List<Category>
        {
            new Category
            {
                Id = Guid.NewGuid(),
                Description = "Alimentação",
                Purpose = CategoryPurpose.Both, // Pode ser despesa ou receita
                CreatedAt = now
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Description = "Transporte",
                Purpose = CategoryPurpose.Expense, // Apenas despesa
                CreatedAt = now
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Description = "Salário",
                Purpose = CategoryPurpose.Income, // Apenas receita
                CreatedAt = now
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Description = "Lazer",
                Purpose = CategoryPurpose.Both, // Pode ser despesa ou receita
                CreatedAt = now
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Description = "Contas e Utilidades",
                Purpose = CategoryPurpose.Expense, // Apenas despesa
                CreatedAt = now
            }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        // Criar 5 transações
        // Garantir que menores de idade só tenham despesas
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = Guid.NewGuid(),
                Description = "Compra no supermercado",
                Value = 250.50m,
                Type = TransactionType.Expense,
                CategoryId = categories[0].Id, // Alimentação
                PersonId = people[0].Id, // João Silva
                CreatedAt = now
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                Description = "Salário mensal",
                Value = 5000.00m,
                Type = TransactionType.Income,
                CategoryId = categories[2].Id, // Salário
                PersonId = people[1].Id, // Maria Santos
                CreatedAt = now
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                Description = "Combustível",
                Value = 150.00m,
                Type = TransactionType.Expense,
                CategoryId = categories[1].Id, // Transporte
                PersonId = people[2].Id, // Pedro Oliveira
                CreatedAt = now
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                Description = "Lanche na escola",
                Value = 15.50m,
                Type = TransactionType.Expense, // Menor de idade só pode ter despesas
                CategoryId = categories[0].Id, // Alimentação
                PersonId = people[3].Id, // Ana Costa (menor de idade)
                CreatedAt = now
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                Description = "Conta de luz",
                Value = 180.75m,
                Type = TransactionType.Expense,
                CategoryId = categories[4].Id, // Contas e Utilidades
                PersonId = people[4].Id, // Carlos Ferreira
                CreatedAt = now
            }
        };

        context.Transactions.AddRange(transactions);
        context.SaveChanges();
    }
}

