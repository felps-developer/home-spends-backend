using HomeSpends.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HomeSpends.Infrastructure;

/// <summary>
/// Factory para criação do DbContext em tempo de design (para migrations).
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Connection string padrão para desenvolvimento
        // Em produção, isso será sobrescrito pela configuração da aplicação
        var connectionString = "Host=localhost;Port=5432;Database=home_spends;Username=postgres;Password=postgres";
        
        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

