# Fision.API

API REST para gestão de entidades (alunos e profissionais), pessoas, contratos financeiros, movimentos financeiros e caixa. O domínio é voltado a clínica ou escola (por exemplo, fisioterapia): cadastro de pessoas, vínculo como Aluno ou Profissional, especialidades, contratos e fluxo de caixa.

## Para que serve o projeto

O **Fision.API** permite:

- **Cadastro de pessoas** – CPF, nome, data de nascimento, sexo, telefone, e-mail e endereço.
- **Entidades** – vínculo da pessoa com a organização (matrícula, datas de entrada/saída, classe Aluno ou Profissional, especialidade, contrato).
- **Gestão financeira** – contratos, movimentos financeiros (por entidade e avulsos) e caixa.

A API é versionada (v1) e expõe recursos como entidades, caixa e movimentos financeiros, com operações CRUD e endpoints específicos (atualizar endereço, contrato e pessoa da entidade). A autenticação é feita via ASP.NET Core Identity (Identity API Endpoints), com banco de dados separado para usuários.

### Principais entidades

| Entidade                                                        | Descrição                                                                           |
| --------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| **Pessoa**                                                      | Dados cadastrais e endereço                                                         |
| **Entidade**                                                    | Vínculo pessoa–organização (Aluno/Profissional), matrícula, especialidade, contrato |
| **Especialidades**                                              | Especialidades disponíveis                                                          |
| **EnderecoPessoa**                                              | Endereço da pessoa                                                                  |
| **ContratoFinanceiro**                                          | Contrato financeiro da entidade                                                     |
| **MovimentoFinanceiroEntidade** / **MovimentoFinanceiroAvulso** | Lançamentos financeiros                                                             |
| **Caixa**                                                       | Controle de caixa                                                                   |

---

## Tecnologias utilizadas

| Camada / Aspecto         | Tecnologia                                                                      |
| ------------------------ | ------------------------------------------------------------------------------- |
| **Runtime / Framework**  | .NET 8.0                                                                        |
| **API**                  | ASP.NET Core (Web API), Startup + Program                                       |
| **Banco de dados**       | SQL Server, Entity Framework Core 8.0.22                                        |
| **ORM / Acesso a dados** | EF Core (DbContext, repositórios), EF Core Tools/Design                         |
| **Autenticação**         | ASP.NET Core Identity (Identity API Endpoints), segundo DbContext para usuários |
| **Documentação API**     | Swagger (Swashbuckle.AspNetCore 10.1.0)                                         |
| **Versionamento API**    | Microsoft.AspNetCore.Mvc.Versioning 5.0.0 (rotas `api/v1/...`)                  |
| **Mapeamento**           | AutoMapper + Extensions.Microsoft.DependencyInjection                           |
| **Validação**            | FluentValidation (projeto Business)                                             |
| **Estrutura**            | Solução em 3 projetos: API, Business, Data                                      |

---

## Arquitetura

Fluxo: **Controller → Service (Business) → Repository (Data) → DbContext → SQL Server**. AutoMapper mapeia ViewModels e entidades; Identity usa um DbContext separado para autenticação.

```mermaid
flowchart LR
  subgraph api [FIsionAPI.API]
    Controllers
    ViewModels
    Auth[Identity Auth]
    Swagger
  end
  subgraph business [FIsionAPI.Business]
    Services
    Models
    Validators
  end
  subgraph data [FIsionAPI.Data]
    Repositories
    FisionContext
  end
  DB[(SQL Server)]
  Controllers --> Services
  Services --> Repositories
  Repositories --> FisionContext
  FisionContext --> DB
  Auth --> AuthDB["Auth DB"]
```

---

## Estrutura da solução

| Projeto                | Descrição                                                                                           |
| ---------------------- | --------------------------------------------------------------------------------------------------- |
| **FIsionAPI.API**      | Controllers (V1), ViewModels, configuração, autenticação, Swagger e injeção de dependências.        |
| **FIsionAPI.Business** | Modelos de domínio, interfaces (repositórios e serviços), serviços e validações (FluentValidation). |
| **FIsionAPI.Data**     | `FisionContext`, repositórios e configurações do EF Core (mapeamentos/migrations).                  |

Dependências: **API** referencia **Data**; **Data** referencia **Business**; **Business** é a camada de domínio e regras de negócio.

---

## Configuração local (banco de dados)

A API usa **dois bancos SQL Server** (duas connection strings):

| Chave                      | Uso                                                              |
| -------------------------- | ---------------------------------------------------------------- |
| `DefaultConnection`        | Domínio (`FisionContext` — entidades, pessoas, financeiro, etc.) |
| `AuthenticationConnection` | Identity (`AuthenticationDbContext` — usuários e perfis)         |

**Não commite senhas ou servidores reais.** O repositório mantém `appsettings.json` com valores vazios; em desenvolvimento use um destes:

1. **Copiar o exemplo**  
   Copie `FIsionAPI.API/connectionstrings.Development.json.example` para `FIsionAPI.API/appsettings.Development.json` e ajuste servidor e nomes dos bancos. Esse arquivo já está no `.gitignore`.

2. **User Secrets (recomendado)**  
   O projeto `FIsionAPI.API` possui `UserSecretsId`; em desenvolvimento o host carrega segredos automaticamente.

   ```bash
   cd FIsionAPI.API
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "SUA_STRING_DOMINIO"
   dotnet user-secrets set "ConnectionStrings:AuthenticationConnection" "SUA_STRING_AUTH"
   ```

3. **Variáveis de ambiente** (útil em CI/Docker): `ConnectionStrings__DefaultConnection` e `ConnectionStrings__AuthenticationConnection`.

### Aplicar migrations (após definir as connection strings)

Na raiz do repositório (ajuste os caminhos se usar só uma pasta do projeto):

```bash
dotnet ef database update --project FIsionAPI.Data --startup-project FIsionAPI.API --context FisionContext

dotnet ef database update --project FIsionAPI.API --startup-project FIsionAPI.API --context AuthenticationDbContext
```

O `startup-project` precisa ser a API para carregar `appsettings` / User Secrets e as connection strings.

Instale a ferramenta global se ainda não tiver: `dotnet tool install --global dotnet-ef` (versão alinhada ao EF Core 8).

### Identity API Endpoints (pipeline)

Com as migrations do `AuthenticationDbContext` aplicadas e a `AuthenticationConnection` configurada, o `Startup` expõe os endpoints do Identity e autentica requisições com **Bearer token**:

- `UseAuthentication()` antes de `UseAuthorization()`
- `MapIdentityApi<User>()` junto de `MapControllers()`

Rotas padrão na raiz da aplicação (ajuste a URL base conforme o ambiente), por exemplo: **`POST /register`** e **`POST /login`** com corpo JSON contendo email/senha conforme o contrato do ASP.NET Core Identity. O token retornado deve ser enviado no header `Authorization: Bearer <token>` quando os controllers forem protegidos com `[Authorize]` (próximas fases).
