using HomeSpends.Application.Services;
using HomeSpends.Domain.Interfaces;
using HomeSpends.Infrastructure.Data;
using HomeSpends.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CONFIGURAÇÃO DE SERVIÇOS
// ============================================

// Adiciona controllers da API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger/OpenAPI para documentação da API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Home Spends API",
        Version = "v1",
        Description = "API para controle de gastos residenciais"
    });
});

// ============================================
// CONFIGURAÇÃO DO BANCO DE DADOS
// ============================================
// Configuração do Entity Framework Core com PostgreSQL
// A connection string é lida do appsettings.json através da chave "ConnectionStrings:DefaultConnection"
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ============================================
// INJEÇÃO DE DEPENDÊNCIAS - REPOSITÓRIOS
// ============================================
// Repositórios são registrados com escopo Scoped (uma instância por requisição HTTP)
// Isso garante que a mesma instância do DbContext seja usada durante toda a requisição
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// ============================================
// INJEÇÃO DE DEPENDÊNCIAS - SERVIÇOS
// ============================================
// Serviços de aplicação que implementam a lógica de negócio
// Também registrados com escopo Scoped para compartilhar o mesmo DbContext
builder.Services.AddScoped<IMapperService, MapperService>();        // Serviço de mapeamento entre entidades e DTOs
builder.Services.AddScoped<IPersonService, PersonService>();         // Serviço de lógica de negócio para pessoas
builder.Services.AddScoped<ICategoryService, CategoryService>();     // Serviço de lógica de negócio para categorias
builder.Services.AddScoped<ITransactionService, TransactionService>(); // Serviço de lógica de negócio para transações
builder.Services.AddScoped<IReportService, ReportService>();        // Serviço de geração de relatórios

// ============================================
// CONFIGURAÇÃO DE CORS
// ============================================
// CORS (Cross-Origin Resource Sharing) permite que o frontend faça requisições para a API
// A política "AllowAll" permite requisições de qualquer origem (apenas para desenvolvimento)
// Em produção, deve-se restringir para origens específicas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()      // Permite qualquer origem
              .AllowAnyMethod()       // Permite qualquer método HTTP (GET, POST, etc.)
              .AllowAnyHeader();     // Permite qualquer header
    });
});

var app = builder.Build();

// ============================================
// CONFIGURAÇÃO DO PIPELINE HTTP
// ============================================

// Swagger/OpenAPI - Documentação interativa da API (apenas em desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Home Spends API v1");
    });
}

// Redireciona requisições HTTP para HTTPS (segurança)
app.UseHttpsRedirection();

// Aplica a política CORS configurada anteriormente
app.UseCors("AllowAll");

// Middleware de tratamento global de exceções
// Captura todas as exceções não tratadas e retorna respostas HTTP padronizadas
// Centraliza o tratamento de erros seguindo o princípio Single Responsibility
app.UseMiddleware<HomeSpends.API.Middleware.GlobalExceptionHandlerMiddleware>();

// Autorização (se necessário no futuro)
app.UseAuthorization();

// Mapeia os controllers da API
app.MapControllers();

// ============================================
// INICIALIZAÇÃO DO BANCO DE DADOS
// ============================================
// Aplica migrations automaticamente e popula dados iniciais (apenas em desenvolvimento)
// Em produção, migrations devem ser aplicadas manualmente ou via CI/CD
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        try
        {
            // Aplica todas as migrations pendentes automaticamente
            // Cria ou atualiza o schema do banco de dados conforme necessário
            dbContext.Database.Migrate();
            
            // Popular o banco com dados iniciais (seed)
            // Cria categorias e pessoas padrão para facilitar testes
            DbSeeder.Seed(dbContext);
            logger.LogInformation("Banco de dados populado com dados iniciais");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao aplicar migrations ou popular banco de dados");
        }
    }
}

app.Run();

