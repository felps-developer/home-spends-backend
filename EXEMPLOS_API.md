# Exemplos de Uso da API

Este documento contém exemplos práticos de como usar a API Home Spends.

## Base URL

- Local: `http://localhost:5000`
- Docker: `http://localhost:5000`

## 1. Criar uma Pessoa

```http
POST /api/people
Content-Type: application/json

{
  "name": "João Silva",
  "age": 25
}
```

**Resposta:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "João Silva",
  "age": 25,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": null
}
```

## 2. Listar Todas as Pessoas

```http
GET /api/people
```

**Resposta:**
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "João Silva",
    "age": 25,
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": null
  },
  {
    "id": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
    "name": "Maria Santos",
    "age": 17,
    "createdAt": "2024-01-15T10:35:00Z",
    "updatedAt": null
  }
]
```

## 3. Criar uma Categoria

```http
POST /api/categories
Content-Type: application/json

{
  "description": "Alimentação",
  "purpose": 3
}
```

**Valores de `purpose`:**
- `1` = Despesa
- `2` = Receita
- `3` = Ambas

**Resposta:**
```json
{
  "id": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
  "description": "Alimentação",
  "purpose": 3,
  "createdAt": "2024-01-15T10:40:00Z",
  "updatedAt": null
}
```

## 4. Listar Todas as Categorias

```http
GET /api/categories
```

## 5. Criar uma Transação

### Exemplo 1: Despesa para Adulto

```http
POST /api/transactions
Content-Type: application/json

{
  "description": "Compra no supermercado",
  "value": 150.50,
  "type": 1,
  "categoryId": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
  "personId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Valores de `type`:**
- `1` = Despesa
- `2` = Receita

### Exemplo 2: Tentativa de Receita para Menor (ERRO)

```http
POST /api/transactions
Content-Type: application/json

{
  "description": "Mesada",
  "value": 50.00,
  "type": 2,
  "categoryId": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
  "personId": "4fa85f64-5717-4562-b3fc-2c963f66afa7"
}
```

**Resposta de Erro:**
```json
{
  "message": "Menores de idade (menor de 18 anos) só podem ter despesas"
}
```

### Exemplo 3: Despesa com Categoria de Receita (ERRO)

```http
POST /api/transactions
Content-Type: application/json

{
  "description": "Compra",
  "value": 100.00,
  "type": 1,
  "categoryId": "categoria-id-de-receita",
  "personId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Resposta de Erro:**
```json
{
  "message": "A categoria 'Salário' não pode ser usada para despesas. Finalidade: Income"
}
```

## 6. Listar Todas as Transações

```http
GET /api/transactions
```

**Resposta:**
```json
[
  {
    "id": "6fa85f64-5717-4562-b3fc-2c963f66afa9",
    "description": "Compra no supermercado",
    "value": 150.50,
    "type": 1,
    "category": {
      "id": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
      "description": "Alimentação",
      "purpose": 3,
      "createdAt": "2024-01-15T10:40:00Z",
      "updatedAt": null
    },
    "person": {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "João Silva",
      "age": 25,
      "createdAt": "2024-01-15T10:30:00Z",
      "updatedAt": null
    },
    "createdAt": "2024-01-15T11:00:00Z",
    "updatedAt": null
  }
]
```

## 7. Relatório de Totais por Pessoa

```http
GET /api/reports/person-totals
```

**Resposta:**
```json
{
  "people": [
    {
      "personId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "personName": "João Silva",
      "totalIncome": 5000.00,
      "totalExpense": 3200.50,
      "balance": 1799.50
    },
    {
      "personId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
      "personName": "Maria Santos",
      "totalIncome": 0.00,
      "totalExpense": 150.00,
      "balance": -150.00
    }
  ],
  "summary": {
    "totalIncome": 5000.00,
    "totalExpense": 3350.50,
    "netBalance": 1649.50
  }
}
```

## 8. Relatório de Totais por Categoria

```http
GET /api/reports/category-totals
```

**Resposta:**
```json
{
  "categories": [
    {
      "categoryId": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
      "categoryDescription": "Alimentação",
      "totalIncome": 0.00,
      "totalExpense": 2500.00,
      "balance": -2500.00
    },
    {
      "categoryId": "7fa85f64-5717-4562-b3fc-2c963f66afaa",
      "categoryDescription": "Salário",
      "totalIncome": 5000.00,
      "totalExpense": 0.00,
      "balance": 5000.00
    }
  ],
  "summary": {
    "totalIncome": 5000.00,
    "totalExpense": 2500.00,
    "netBalance": 2500.00
  }
}
```

## 9. Deletar uma Pessoa

```http
DELETE /api/people/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Resposta:** `204 No Content`

**Nota:** Ao deletar uma pessoa, todas as transações dessa pessoa são deletadas automaticamente.

## Fluxo Completo de Exemplo

1. Criar pessoas:
   - João (25 anos)
   - Maria (17 anos - menor de idade)

2. Criar categorias:
   - Alimentação (Ambas)
   - Salário (Receita)
   - Contas (Despesa)

3. Criar transações:
   - João: Receita de R$ 5000 (Salário)
   - João: Despesa de R$ 150 (Alimentação)
   - Maria: Despesa de R$ 50 (Alimentação) ✅
   - Maria: Receita de R$ 100 (Salário) ❌ ERRO: Menor não pode ter receita

4. Gerar relatórios:
   - Totais por pessoa
   - Totais por categoria

## Códigos de Status HTTP

- `200 OK` - Sucesso
- `201 Created` - Recurso criado com sucesso
- `204 No Content` - Sucesso sem conteúdo (deleção)
- `400 Bad Request` - Dados inválidos ou regra de negócio violada
- `404 Not Found` - Recurso não encontrado
- `500 Internal Server Error` - Erro interno do servidor

