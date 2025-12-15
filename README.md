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

## Como Executar

### Usando Docker Compose (Recomendado)

1. Clone o repositório
2. Execute o comando:

```bash
docker-compose up -d
```

Isso irá:
- Criar e iniciar o container do PostgreSQL
- Criar e iniciar o container da API
- Aplicar as migrations automaticamente

A API estará disponível em: `http://localhost:5000`
Swagger UI: `http://localhost:5000/swagger`

### Executando Localmente

1. Certifique-se de que o PostgreSQL está rodando
2. Configure a connection string no `appsettings.json`
3. Execute as migrations:

```bash
cd src/HomeSpends.API
dotnet ef database update
```

4. Execute a aplicação:

```bash
dotnet run
```

## Migrations

### Criar uma nova migration

```bash
cd src/HomeSpends.API
dotnet ef migrations add NomeDaMigration --project ../HomeSpends.Infrastructure
```

### Aplicar migrations

```bash
dotnet ef database update
```

### Reverter migration

```bash
dotnet ef database update NomeDaMigrationAnterior
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

## Licença

Este projeto foi desenvolvido como parte de um teste técnico.

