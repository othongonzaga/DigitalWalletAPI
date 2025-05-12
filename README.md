
# 💸 Digital Wallet API

Uma API RESTful desenvolvida em C# com ASP.NET Core para gerenciamento de **carteiras digitais** e **transações financeiras seguras**, com autenticação JWT, arquitetura em camadas (MVC) e Docker para conteinerização.

---

## 🧠 Resumo do Projeto

O projeto **Digital Wallet API** tem como objetivo oferecer uma estrutura robusta para:

- Criar e gerenciar carteiras digitais.
- Realizar transferências entre carteiras.
- Proteger as rotas com autenticação baseada em JWT.
- Armazenar e consultar transações realizadas entre os usuários.

---

## ⚙️ Tecnologias Utilizadas

- **C# / .NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core** (ORM)
- **PostgreSQL** (Banco relacional)
- **JWT (JSON Web Token)** para autenticação
- **Docker** e **Docker Compose**
- **Swagger** para documentação de endpoints
---

## 🏗️ Arquitetura do Projeto

A aplicação segue a arquitetura MVC com separação clara de responsabilidades:

```
📁 Controllers       // Entrada de requisições HTTP (ex: AuthController, WalletController)
📁 Services          // Validação (ex: AuthService)
📁 Models            // Entidades do banco (ex: User, Wallet, Transaction)
📁 DTOs              // Data Transfer Objects para entrada e saída de dados
📁 Data              // DbContext
📁 Migrations        // Migrations

```

---

## 🚀 Funcionalidades da API

### ✅ Autenticação
- Login com e-mail e senha
- Geração de token JWT

### 👤 Usuários
- Cadastro de usuário, pelo `api/auth/register`
- Recuperação de dados do usuário logado (via token), pelo `/api/user`

### 💼 Carteiras
- Criação de carteira para o usuário
- Consulta de saldo
- Listagem de carteira

### 💸 Transações
- Transferência entre carteiras
- Histórico de transações realizadas e recebidas
---

## 📦 Como Executar o Projeto

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker e Docker Compose](https://www.docker.com/products/docker-desktop)
- (Opcional) [Postman](https://www.postman.com/) para testar os endpoints

### 🔧 Configuração Local

1. **Clone o repositório**
   ```bash
   git clone https://github.com/othongonzaga/DigitalWalletAPI.git
   cd DigitalWalletAPI
   ```

2. **Configure o arquivo `appsettings.json` :**

```json
"JWT": {
  "Issuer": "DigitalWalletAPI",
  "Audience": "DigitalWalletAPI",
  "Secret": "aU7x@9mP!rB3tY2#qD8zK6jW^vL4oE1s"
}
```

> ❗ Importante: a chave JWT deve ter ao menos 256 bits (32 caracteres) para funcionar com o algoritmo HS256.

3. **Execute o projeto com Docker Compose**

```bash
docker-compose up --build
```

A aplicação estará disponível em: `http://localhost:5000`

4. **Acesse a documentação Swagger**

```
http://localhost:5000/swagger
```

5. **Exemplo de rota de login**

```
POST http://localhost:5000/api/Auth/login
```

```json
{
  "email": "usuario@example.com",
  "passwordHash": "senha123"
}
```

---

## 🧪 Dados Fictícios

Ao subir o container, o banco é populado com dados de demonstração automaticamente, incluindo:

- Usuários com carteiras
- Transações simuladas

---

## 🔐 Segurança

- As senhas dos usuários são armazenadas usando hash.
- As rotas protegidas exigem **Bearer Token JWT** no cabeçalho `Authorization`.

---

## 📂 Scripts Úteis

- **Criação do banco**: EF Core migrations automatizadas na inicialização
- **Seed de dados fictícios**
- **Comando manual para migração**:
  ```bash
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```

---

## 👨‍💻 Autor

Desenvolvido por [Othon Gonzaga](https://github.com/othongonzaga)

---

## 📄 Licença

Este projeto está sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

---

## 📺 Imagens de teste no Postman 

![img 1](https://github.com/user-attachments/assets/dd90d120-eb78-4e74-8699-a9c2ea1ae68e)
![img 2](https://github.com/user-attachments/assets/7ef4513a-11ca-4e56-a209-799a96d2b48f)
![img 3](https://github.com/user-attachments/assets/9ff92973-2823-420b-a477-871241fca05e)


