# Home Spends Backend

Sistema de controle de gastos residenciais desenvolvido em .NET 8 com Entity Framework Core, PostgreSQL e Docker.

## Arquitetura

O projeto segue os princípios de **Clean Architecture** e **SOLID**, organizado em camadas:

- **Domain**: Entidades, interfaces e regras de negócio
- **Application**: Serviços, DTOs e lógica de aplicação
- **Infrastructure**: Repositórios, DbContext e acesso a dados
- **API**: Controllers e configuração da aplicação

## Tecnologias

- .NET 8.0
- Entity Framework Core 8.0
- PostgreSQL
- Docker & Docker Compose
- Swagger/OpenAPI

## Estrutura do Projeto

```
home-spends-backend/
├── src/
│   ├── HomeSpends.API/          # Camada de apresentação (Controllers)
│   ├── HomeSpends.Application/   # Camada de aplicação (Services, DTOs)
│   ├── HomeSpends.Domain/         # Camada de domínio (Entities, Interfaces)
│   └── HomeSpends.Infrastructure/ # Camada de infraestrutura (Repositories, DbContext)
├── docker-compose.yml
├── Dockerfile
└── HomeSpends.sln
```

## Funcionalidades

### Cadastro de Pessoas
- Criação, listagem e deleção de pessoas
- Ao deletar uma pessoa, todas as transações são deletadas automaticamente (Cascade)
- Campos: ID (auto-gerado), Nome, Idade

### Cadastro de Categorias
- Criação e listagem de categorias
- Campos: ID (auto-gerado), Descrição, Finalidade (Despesa/Receita/Ambas)

### Cadastro de Transações
- Criação e listagem de transações
- Validações:
  - Menores de idade (menor de 18 anos) só podem ter despesas
  - A categoria deve permitir o tipo de transação (despesa/receita)
- Campos: ID (auto-gerado), Descrição, Valor, Tipo, Categoria, Pessoa

### Relatórios
- **Totais por Pessoa**: Lista todas as pessoas com totais de receitas, despesas e saldo
- **Totais por Categoria**: Lista todas as categorias com totais de receitas, despesas e saldo
- Ambos incluem resumo geral no final

## Pré-requisitos

- .NET 8.0 SDK
- Docker e Docker Compose
- PostgreSQL (ou usar via Docker)
- Git (para clonar o repositório)
- EF Core Tools (para executar migrations localmente): `dotnet tool install --global dotnet-ef`

## Como Executar

### Usando Docker Compose (Recomendado)

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
cd home-spends-backend
```

2. Execute o comando:

```bash
docker-compose up -d
```

Isso irá:
- Criar e iniciar o container do PostgreSQL
- Aguardar o PostgreSQL estar pronto (healthcheck)
- Criar e iniciar o container da API
- Aplicar as migrations automaticamente
- Popular o banco com dados iniciais (seed)

**Aguarde alguns segundos** para que os containers iniciem completamente.

A API estará disponível em: `http://localhost:5000`
Swagger UI: `http://localhost:5000/swagger`

**Verificar logs (se necessário):**
```bash
# Ver logs de todos os serviços
docker-compose logs -f

# Ver logs apenas da API
docker-compose logs -f api

# Ver logs apenas do PostgreSQL
docker-compose logs -f postgres
```

**Parar os containers:**
```bash
docker-compose down
```

**Parar e remover volumes (limpar dados):**
```bash
docker-compose down -v
```

### Executando Localmente

1. **Certifique-se de que o PostgreSQL está rodando** e acessível

2. **Configure a connection string** no `appsettings.json` ou `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=home_spends;Username=postgres;Password=postgres"
  }
}
```

3. **Instale o EF Core Tools** (se ainda não tiver):
```bash
dotnet tool install --global dotnet-ef
```

4. **Restaure as dependências**:
```bash
cd src/HomeSpends.API
dotnet restore
```

5. **Execute as migrations**:
```bash
dotnet ef database update --project ../HomeSpends.Infrastructure
```

6. **Execute a aplicação**:
```bash
dotnet run
```

A API estará disponível em: `http://localhost:5000` (ou a porta configurada no `launchSettings.json`)
Swagger UI: `http://localhost:5000/swagger`

## Migrations

**Nota:** Certifique-se de ter o EF Core Tools instalado: `dotnet tool install --global dotnet-ef`

### Criar uma nova migration

```bash
cd src/HomeSpends.API
dotnet ef migrations add NomeDaMigration --project ../HomeSpends.Infrastructure
```

### Aplicar migrations

```bash
cd src/HomeSpends.API
dotnet ef database update --project ../HomeSpends.Infrastructure
```

### Reverter migration

```bash
cd src/HomeSpends.API
dotnet ef database update NomeDaMigrationAnterior --project ../HomeSpends.Infrastructure
```

### Listar migrations aplicadas

```bash
cd src/HomeSpends.API
dotnet ef migrations list --project ../HomeSpends.Infrastructure
```

## Endpoints da API

### Pessoas
- `GET /api/people` - Lista todas as pessoas
- `GET /api/people/{id}` - Busca pessoa por ID
- `POST /api/people` - Cria nova pessoa
- `DELETE /api/people/{id}` - Deleta pessoa (e suas transações)

### Categorias
- `GET /api/categories` - Lista todas as categorias
- `GET /api/categories/{id}` - Busca categoria por ID
- `POST /api/categories` - Cria nova categoria

### Transações
- `GET /api/transactions` - Lista todas as transações
- `GET /api/transactions/{id}` - Busca transação por ID
- `POST /api/transactions` - Cria nova transação

### Relatórios
- `GET /api/reports/person-totals` - Totais por pessoa
- `GET /api/reports/category-totals` - Totais por categoria

## Exemplos de Uso

### Criar uma Pessoa

```json
POST /api/people
{
  "name": "João Silva",
  "age": 25
}
```

### Criar uma Categoria

```json
POST /api/categories
{
  "description": "Alimentação",
  "purpose": 3
}
```

Onde `purpose` pode ser:
- `1` = Despesa
- `2` = Receita
- `3` = Ambas

### Criar uma Transação

```json
POST /api/transactions
{
  "description": "Compra no supermercado",
  "value": 150.50,
  "type": 1,
  "categoryId": "guid-da-categoria",
  "personId": "guid-da-pessoa"
}
```

Onde `type` pode ser:
- `1` = Despesa
- `2` = Receita

## Regras de Negócio Implementadas

1. **Deleção em Cascata**: Ao deletar uma pessoa, todas as transações são deletadas automaticamente
2. **Restrição para Menores**: Menores de idade (menor de 18 anos) só podem ter despesas
3. **Validação de Categoria**: A categoria deve permitir o tipo de transação (despesa/receita)
4. **Valor Positivo**: O valor da transação deve ser positivo (validado no banco de dados)

## Troubleshooting

### Problemas comuns

**Erro ao executar `docker-compose up`:**
- Verifique se as portas 5000 e 5432 não estão em uso
- Verifique se o Docker está rodando
- Execute `docker-compose down` e tente novamente

**Erro de conexão com o banco:**
- Aguarde alguns segundos para o PostgreSQL iniciar completamente
- Verifique os logs: `docker-compose logs postgres`
- Verifique se a connection string está correta

**Erro ao executar migrations localmente:**
- Certifique-se de ter o EF Core Tools instalado: `dotnet tool install --global dotnet-ef`
- Verifique se está no diretório correto (`src/HomeSpends.API`)
- Verifique se a connection string está correta no `appsettings.json`
- Certifique-se de que o PostgreSQL está rodando e acessível

**Erro "Cannot find project":**
- Certifique-se de estar no diretório `src/HomeSpends.API`
- Use o caminho relativo correto: `--project ../HomeSpends.Infrastructure`

**API não inicia:**
- Verifique os logs: `docker-compose logs api`
- Verifique se o PostgreSQL está acessível
- Verifique se as migrations foram aplicadas

## Desenvolvimento

### Estrutura de Código

O código segue os princípios SOLID:
- **Single Responsibility**: Cada classe tem uma única responsabilidade
- **Open/Closed**: Aberto para extensão, fechado para modificação
- **Liskov Substitution**: Interfaces bem definidas
- **Interface Segregation**: Interfaces específicas por domínio
- **Dependency Inversion**: Dependências através de interfaces

### Padrões Utilizados

- **Repository Pattern**: Abstração do acesso a dados
- **Service Layer**: Lógica de negócio separada dos controllers
- **DTO Pattern**: Transferência de dados entre camadas
- **Dependency Injection**: Injeção de dependências via construtores



