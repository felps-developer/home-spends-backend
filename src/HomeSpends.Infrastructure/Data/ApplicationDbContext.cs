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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Person
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Age).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => e.Id).IsUnique();

            // Relacionamento: Uma pessoa pode ter várias transações
            entity.HasMany(e => e.Transactions)
                  .WithOne(e => e.Person)
                  .HasForeignKey(e => e.PersonId)
                  .OnDelete(DeleteBehavior.Cascade); // Ao deletar pessoa, deleta transações
        });

        // Configuração da entidade Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Purpose).IsRequired().HasConversion<int>();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => e.Id).IsUnique();

            // Relacionamento: Uma categoria pode ter várias transações
            entity.HasMany(e => e.Transactions)
                  .WithOne(e => e.Category)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict); // Não permite deletar categoria com transações
        });

        // Configuração da entidade Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Value).IsRequired().HasPrecision(18, 2);
            entity.Property(e => e.Type).IsRequired().HasConversion<int>();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => e.Id).IsUnique();
            entity.HasIndex(e => e.PersonId);
            entity.HasIndex(e => e.CategoryId);

            // Validação: Valor deve ser positivo
            entity.HasCheckConstraint("CK_Transaction_Value_Positive", "[Value] > 0");
        });
    }
}

