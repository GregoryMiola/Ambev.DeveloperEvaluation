# Como Executar e Testar o Projeto

Este guia fornece as instruções para configurar, executar e testar a API de Vendas de forma padronizada e independente de sistema operacional.

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/) e Docker Compose

### 1. Configuração do Ambiente (Obrigatório)

O projeto utiliza Docker Compose para orquestrar a API e o banco de dados PostgreSQL. As configurações de conexão são gerenciadas por variáveis de ambiente, que devem ser fornecidas em um arquivo `.env`.

1.  Na pasta `template/backend`, crie um arquivo chamado `.env`.
2.  Copie o conteúdo abaixo para o seu arquivo `.env`:

```env
# PostgreSQL Settings
POSTGRES_DB=salesdb
POSTGRES_USER=user
POSTGRES_PASSWORD=password
POSTGRES_DB_HOST=ambev.developerevaluation.database
POSTGRES_DB_PORT=5432
```

### 2. Executando a Aplicação (Via Docker)

Com o Docker e o arquivo .env configurados, execute o seguinte comando na pasta template/backend:

```
docker-compose up --build
```

Este comando irá:

    * Construir a imagem Docker da API.
    * Iniciar os contêineres da API e do banco de dados PostgreSQL.
    * A API, ao iniciar, aplicará automaticamente as migrações do Entity Framework no banco de dados.

Após a inicialização, a API estará disponível:

    * Swagger UI (Documentação da API): 
    http://localhost:8080/swagger

### 3. Executando os Testes (Localmente)
A solução contém uma suíte completa de testes (Unitários, Integração e Funcionais). Para executá-los, navegue até a pasta template/backend/src e use o seguinte comando:

```
dotnet test Ambev.DeveloperEvaluation.sln
```

Isso irá restaurar as dependências, compilar a solução e rodar todos os testes, garantindo a qualidade e o comportamento esperado da aplicação.