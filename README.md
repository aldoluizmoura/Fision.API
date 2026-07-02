# Fision.API

API REST para gestão de entidades (alunos e profissionais), pessoas, contratos financeiros, movimentos financeiros e caixa. O domínio é voltado a clínica ou escola (por exemplo, fisioterapia): cadastro de pessoas, vínculo como Aluno ou Profissional, especialidades, contratos e fluxo de caixa.

## Para que serve o projeto

O **Fision.API** permite:

- **Cadastro de pessoas** – CPF, nome, data de nascimento, sexo, telefone, e-mail e endereço.
- **Entidades** – vínculo da pessoa com a organização (matrícula, datas de entrada/saída, classe Aluno ou Profissional, especialidade, contrato).
- **Gestão financeira** – contratos, movimentos financeiros (por entidade e avulsos) e caixa.

A API é versionada (v1) e expõe recursos como entidades, caixa, movimentos financeiros e especialidades. A autenticação usa **ASP.NET Core Identity** com **JWT Bearer** e **refresh token**, em banco de dados separado do domínio.

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
| **Autenticação**         | ASP.NET Core Identity + JWT Bearer + refresh token; `AuthenticationDbContext` separado |
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
    Auth[JWT + Identity]
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

---

## Autenticação e autorização

A camada de acesso combina **Identity** (usuários, senhas e roles) com **JWT** (tokens stateless nas requisições). O domínio da aplicação e os usuários ficam em bancos distintos.

### Arquitetura de acesso

```mermaid
flowchart TB
  Client[Cliente HTTP]
  AuthCtrl[AuthController]
  Identity[ASP.NET Identity]
  TokenSvc[TokenService]
  AuthDB[(AuthenticationDbContext)]
  API[Controllers protegidos]
  Policies[Policies RequerAdmin / RequerGestor]

  Client -->|POST registrar/login| AuthCtrl
  AuthCtrl --> Identity
  Identity --> AuthDB
  AuthCtrl --> TokenSvc
  TokenSvc --> AuthDB
  Client -->|Authorization Bearer| API
  API --> Policies
  Policies --> Identity
```

### Pipeline HTTP

Ordem no `Startup.Configure`:

1. `UseAuthentication()` — valida o JWT
2. `UseAuthorization()` — aplica roles e policies
3. `MapControllers()`

**Fallback policy:** todo endpoint exige usuário autenticado, exceto os marcados com `[AllowAnonymous]`.

### Roles (perfis)

| Role      | Descrição                                      |
| --------- | ---------------------------------------------- |
| `Admin`   | Acesso total; gestão de usuários e exclusões   |
| `Gestor`  | Operações financeiras (caixa e movimentos)     |
| `Usuario` | Cadastros básicos (entidades, leitura)         |

As roles são criadas automaticamente na inicialização (`IdentityDataSeeder`). Um usuário **Admin** inicial pode ser semeado via seção `AdminSeed` no `appsettings` (veja exemplo em `appsettings.Development.example.json`).

No **registro público** (`POST /api/v1/auth/registrar`), o usuário recebe automaticamente a role `Usuario`.

### Policies de autorização

| Policy           | Roles aceitas              | Uso principal                          |
| ---------------- | -------------------------- | -------------------------------------- |
| `RequerAdmin`    | Admin                      | Usuários, exclusões críticas           |
| `RequerGestor`   | Admin, Gestor              | Caixa, movimentos financeiros          |
| `RequerUsuario`  | Admin, Gestor, Usuario     | Definida, ainda não usada em controllers |

`Admin` herda permissões de `Gestor` nas policies que listam ambas.

### Configuração JWT (`JwtSettings`)

| Chave                        | Descrição                              |
| ---------------------------- | -------------------------------------- |
| `Issuer`                     | Emissor do token                       |
| `Audience`                   | Audiência válida                       |
| `SecretKey`                  | Chave simétrica (mínimo 32 caracteres) |
| `ExpirationMinutes`          | Validade do access token (padrão: 60)  |
| `RefreshTokenExpirationDays` | Validade do refresh token (padrão: 7)  |

Configure via `appsettings.Development.json`, User Secrets ou variáveis de ambiente (`JwtSettings__SecretKey`, etc.).

### Regras de senha e bloqueio

- Mínimo 8 caracteres, com maiúscula, minúscula, dígito e caractere especial
- E-mail único por usuário
- 5 tentativas de login falhas → bloqueio por 15 minutos
- Usuário com `Ativo = false` não consegue autenticar

### Claims no access token

Além das roles, o JWT pode incluir:

| Claim        | Conteúdo                          |
| ------------ | --------------------------------- |
| `sub`        | Id do usuário (Identity)          |
| `email`      | E-mail                            |
| `nome`       | Nome amigável                     |
| `documento`  | Documento (quando informado)      |
| `pessoaId`   | Vínculo opcional com `Pessoa` do domínio |
| `role`       | Uma claim por role do usuário     |

### Endpoints de autenticação (`/api/v1/auth`)

| Método | Rota             | Auth        | Descrição                          |
| ------ | ---------------- | ----------- | ---------------------------------- |
| POST   | `/registrar`     | Público     | Cria usuário (role `Usuario`) + JWT |
| POST   | `/login`         | Público     | Autentica e retorna JWT            |
| POST   | `/refresh-token` | Público     | Renova access token                |
| POST   | `/logout`        | Autenticado | Revoga refresh token               |

#### Exemplo: login

**Request**

```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "email": "usuario@exemplo.com",
  "senha": "Senha@123"
}
```

**Response (sucesso)**

```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "tokenType": "Bearer",
    "expiresIn": 3600,
    "expiresAt": "2026-07-01T15:00:00Z",
    "refreshToken": "...",
    "refreshTokenExpiresAt": "2026-07-08T14:00:00Z"
  }
}
```

#### Uso do token nas demais requisições

```http
GET /api/v1/entidades
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

No Swagger (ambiente Development), use o botão **Authorize** e informe `Bearer {seu_token}`.

### Gestão de usuários (`/api/v1/usuarios` — somente Admin)

| Método | Rota                  | Descrição                    |
| ------ | --------------------- | ---------------------------- |
| GET    | `/`                   | Listar (filtros: email, nome)|
| GET    | `/{id}`               | Obter por id                 |
| PUT    | `/{id}`               | Atualizar nome e documento   |
| PUT    | `/{id}/ativar`        | Ativar usuário               |
| PUT    | `/{id}/desativar`     | Desativar usuário            |
| PUT    | `/resetar-senha`      | Resetar senha (admin)        |
| PUT    | `/gerenciar-roles`    | Atribuir/remover roles       |

### Matriz de acesso por recurso

Legenda: **Público** = sem token; **Auth** = qualquer autenticado; **Gestor** = Admin ou Gestor; **Admin** = somente Admin.

| Recurso / endpoint base              | Leitura | Criação / alteração | Exclusão |
| ------------------------------------ | ------- | ------------------- | -------- |
| `auth` (registrar, login, refresh)   | Público | Público             | —        |
| `auth/logout`                        | —       | Auth                | —        |
| `entidades`                          | Auth    | Auth                | Admin    |
| `especialidades`                     | Auth    | Gestor              | Admin    |
| `caixa`                              | Gestor  | Gestor              | Admin    |
| `movimento-financeiro`               | Gestor  | Gestor              | Admin    |
| `usuarios`                           | Admin   | Admin               | —        |

#### Detalhamento por perfil

**Usuario (role padrão no registro)**

- Listar e consultar entidades
- Criar e atualizar entidades (inclui pessoa, endereço e contrato)
- Listar e consultar especialidades
- Não acessa caixa, movimentos financeiros nem gestão de usuários

**Gestor**

- Tudo que `Usuario` acessa
- CRUD de especialidades (exceto exclusão)
- Caixa: listar, criar, fechar e reabrir
- Movimentos: mensalidade, profissional, avulso, quitar e desquitar

**Admin**

- Tudo que `Gestor` acessa
- Excluir entidades, especialidades, caixas e movimentos
- Gestão completa de usuários (ativar, desativar, senha, roles)

### Resposta de erro de autorização

- **401 Unauthorized** — token ausente, inválido ou expirado
- **403 Forbidden** — autenticado, mas sem role/policy necessária

Erros de negócio e validação seguem o envelope `{ "success": false, "erros": [...] }` via `BaseController`.

### Limitações atuais

- Policy `RequerUsuario` definida, mas não aplicada em controllers
- Sem endpoint para vincular `User.PessoaId` à pessoa do domínio
- Sem fluxo de recuperação de senha pelo próprio usuário (apenas reset via admin)
- Confirmação de e-mail desabilitada (`RequireConfirmedEmail = false`)
- Em produção, configure `RequireHttpsMetadata = true` no JWT Bearer

### Testes automatizados

O projeto `FIsionAPI.Tests` contém testes unitários das regras de negócio. O CI executa `dotnet test` após o build.
