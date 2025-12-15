using HomeSpends.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeSpends.Infrastructure.Data;

/// <summary>
/// Contexto do Entity Framework Core para o banco de dados.
/// Define as entidades e seus relacionamentos.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet para a entidade Person.
    /// </summary>
    public DbSet<Person> People { get; set; }

    /// <summary>
    /// DbSet para a entidade Category.
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// DbSet para a entidade Transaction.
    /// </summary>
    public DbSet<Transaction> Transactions { get; set; }

    /// <summary>
    /// Configura o modelo de dados (Fluent API).
    /// Define chaves primárias, relacionamentos, índices e constraints.
    /// </summary>
    /// <param name="modelBuilder">Builder usado para configurar o modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============================================
        // CONFIGURAÇÃO DA ENTIDADE PERSON
        // ============================================
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);                                    // Define Id como chave primária
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200); // Nome obrigatório, máximo 200 caracteres
            entity.Property(e => e.Age).IsRequired();                    // Idade obrigatória
            entity.Property(e => e.CreatedAt).IsRequired();              // Data de criação obrigatória
            entity.HasIndex(e => e.Id).IsUnique();                        // Índice único no Id

            // Relacionamento: Uma pessoa pode ter várias transações (1:N)
            // DeleteBehavior.Cascade: Ao deletar uma pessoa, todas suas transações são deletadas automaticamente
            // Isso garante integridade referencial e evita transações órfãs
            entity.HasMany(e => e.Transactions)
                  .WithOne(e => e.Person)
                  .HasForeignKey(e => e.PersonId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ============================================
        // CONFIGURAÇÃO DA ENTIDADE CATEGORY
        // ============================================
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);                                    // Define Id como chave primária
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200); // Descrição obrigatória, máximo 200 caracteres
            entity.Property(e => e.Purpose).IsRequired().HasConversion<int>();     // Purpose é um enum, convertido para int no banco
            entity.Property(e => e.CreatedAt).IsRequired();              // Data de criação obrigatória
            entity.HasIndex(e => e.Id).IsUnique();                        // Índice único no Id

            // Relacionamento: Uma categoria pode ter várias transações (1:N)
            // DeleteBehavior.Restrict: Não permite deletar uma categoria que possui transações associadas
            // Isso protege a integridade dos dados históricos
            entity.HasMany(e => e.Transactions)
                  .WithOne(e => e.Category)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ============================================
        // CONFIGURAÇÃO DA ENTIDADE TRANSACTION
        // ============================================
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);                                    // Define Id como chave primária
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500); // Descrição obrigatória, máximo 500 caracteres
            entity.Property(e => e.Value).IsRequired().HasPrecision(18, 2);    // Valor obrigatório, 18 dígitos totais, 2 decimais
            entity.Property(e => e.Type).IsRequired().HasConversion<int>();     // Type é um enum, convertido para int no banco
            entity.Property(e => e.CreatedAt).IsRequired();              // Data de criação obrigatória
            entity.HasIndex(e => e.Id).IsUnique();                        // Índice único no Id
            entity.HasIndex(e => e.PersonId);                             // Índice para melhorar performance de buscas por pessoa
            entity.HasIndex(e => e.CategoryId);                           // Índice para melhorar performance de buscas por categoria

            // Constraint de validação: Valor deve ser positivo (> 0)
            // Esta validação é aplicada no nível do banco de dados para garantir integridade
            entity.ToTable(t => t.HasCheckConstraint("CK_Transaction_Value_Positive", "\"Value\" > 0"));
        });
    }
}

