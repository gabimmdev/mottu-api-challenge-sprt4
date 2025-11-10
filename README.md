# Mottu API .NET

## Funcionalidades Implementadas

O projeto foi atualizado para incluir as seguintes funcionalidades, conforme os requisitos:

| Funcionalidade | Pontuação | Status |
| :--- | :--- | :--- |
| **Health Checks** | 10 pts | Implementado no endpoint `/health`. |
| **Versionamento da API** | 10 pts | Implementado com `ApiVersioning` (versão `v1.0`). |
| **Segurança de API (JWT)** | 25 pts | Implementado com autenticação **JWT Bearer** para proteger endpoints. |
| **Endpoint com ML.NET** | 25 pts | Implementado um endpoint `/api/v1/MotoPrediction/predict` que usa o ML.NET para prever se uma moto é de alta performance. |
| **Testes Unitários com xUnit** | 30 pts | Implementados testes unitários para a lógica de autenticação e testes de integração básicos (Health Check e Versionamento) com `WebApplicationFactory`. |

## Requisitos

Para executar este projeto, você precisará ter instalado:

*   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) .

## Configuração e Execução

### 1. Clonar e Restaurar Dependências

Após descompactar o arquivo, navegue até o diretório raiz que contém o arquivo `mottu-api-updated.sln` e execute:

```bash
dotnet restore
```

### 2. Executar a API

Para iniciar a API, execute o comando a partir do diretório raiz:

```bash
dotnet run --project MottuBackend
```

A API estará disponível em `http://localhost:5248`.

### 3. Acessar a Documentação (Swagger)

A documentação interativa da API (Swagger UI) estará disponível em:

```
https://localhost:5248/swagger/index.html
```

## Execução dos Testes

O projeto inclui um projeto de testes (`MottuBackend.Tests`) com testes unitários para a lógica de autenticação e testes de integração para as novas funcionalidades (Health Check e Versionamento).

Para executar todos os testes, execute o seguinte comando no diretório raiz (onde está o arquivo `.sln`):

```bash
dotnet test
```

**Resultado Esperado:**

Você deve ver uma saída similar a esta, indicando que todos os testes foram aprovados:

```
Passed!  - Failed:     0, Passed:     7, Skipped:     0, Total:     7, Duration: X s
```

## Exemplos de Teste no Postman

Para testar as funcionalidades, siga os passos de autenticação para obter um token JWT, que é necessário para os endpoints protegidos.

### Passo 1: Obter Token JWT

1.  **Registro (Opcional, se o usuário não existir):**
    *   **Método:** `POST`
    *   **URL:** `https://localhost:7001/api/Auth/register`
    *   **Body (JSON):** `{"username": "testeuser", "password": "password123"}`

2.  **Login (Para obter o Token):**
    *   **Método:** `POST`
    *   **URL:** `https://localhost:7001/api/Auth/login`
    *   **Body (JSON):** `{"username": "testeuser", "password": "password123"}`
    *   **Resultado:** Copie o valor do campo `Token`.

### Passo 2: Testar Health Check

*   **Método:** `GET`
*   **URL:** `https://localhost:7001/health`
*   **Resultado:** Status `200 OK`.

### Passo 3: Testar Predição ML.NET (Protegido)

*   **Método:** `POST`
*   **URL:** `https://localhost:7001/api/v1/MotoPrediction/predict`
*   **Authorization:** Selecione **Bearer Token** e cole o token obtido no Passo 1.
*   **Body (JSON - Exemplo de Alta Performance):**
    ```json
    {
      "cilindrada": 1000,
      "potencia": 150,
      "peso": 200
    }
    ```
*   **Resultado:** Status `200 OK` com a predição.

### Passo 4: Testar Endpoint Versionado (Protegido)

*   **Método:** `GET`
*   **URL:** `https://localhost:7001/api/Motos?api-version=1.0`
*   **Authorization:** Selecione **Bearer Token** e cole o token obtido no Passo 1.
*   **Resultado:** Status `200 OK` com a lista de motos.
