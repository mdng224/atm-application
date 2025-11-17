# 🏧 atm-application

Design and implement a web-based ATM application that allows a user to manage two bank accounts.  
This repo showcases a small but well-structured .NET + Vue app using **ASP.NET Aspire**, **EF Core**, and a modern frontend stack.

<img width="831" height="1205" alt="image" src="https://github.com/user-attachments/assets/6c2b816a-3cc2-4798-9d81-65881a6b60af" />

---

## 🚀 Tech Stack

**Backend**

- .NET / ASP.NET Core (minimal APIs)
- ASP.NET Aspire (`App.AppHost`) for orchestration
- Entity Framework Core + PostgreSQL
- Clean-ish architecture:
  - `App.Domain`
  - `App.Application`
  - `App.Infrastructure`
  - `App.Api`

**Frontend**

- Vue 3 + TypeScript
- Vite
- Tailwind CSS
- TanStack Table (Vue) for data grid

---

## 🧱 Solution Layout

- `App.Api` – HTTP API endpoints (ATM endpoints, contracts, HTTP result mappers)
- `App.AppHost` – Aspire host that wires up API + infrastructure (Postgres, etc.)
- `App.Application` – application layer (CQRS handlers, DTOs, results, pagination)
- `App.Domain` – domain model (`Account`, `Transaction`, `TransactionType`)
- `App.Infrastructure` – EF Core DbContext, migrations, repositories, readers, seeding
- `App.ServiceDefaults` – shared Aspire / service defaults
- `App.Tests` – backend tests
- `frontend` – Vue 3 SPA

---

## 💾 Domain Overview

- **Account**
  - `Id`, `Name`, `Balance`
  - Methods: `Deposit`, `Withdraw`, `TransferTo`
  - Maintains an in-memory list of `Transaction` entities

- **Transaction**
  - `Id`, `AccountId`, `Type` (`Deposit`, `Withdrawal`, `TransferIn`, `TransferOut`)
  - `Amount`, `OccurredAtUtc`, `Description`, `CounterpartyAccountId`

The backend exposes endpoints to:

- Get accounts
- Deposit into an account
- Withdraw from an account
- Transfer between accounts
- Get paged transactions for an account

The frontend shows:

- Current balances for both accounts
- A transfer form
- Deposit/withdraw forms
- A paged “Recent Transactions” table per account

---

## 🧰 Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/) (8+)
- [Node.js](https://nodejs.org/) (v18+ recommended)
- npm
- Docker Desktop (optional, but recommended if you want to run Postgres via Aspire/docker-compose)

---

## 🔧 Backend Setup

From the repo root:

```bash
# Restore & build
dotnet restore
dotnet build
```

---

## 1. Run EF Core Migrations

Migrations live in App.Infrastructure. Apply them using the API project as the startup project:

```bash
dotnet ef database update \
  --project App.Infrastructure \
  --startup-project App.Api
```
This will create and update the Postgres database to match the current model.

If you change the model later, just add a new migration and run database update again.

## 2. Run the backend via Aspire

You can either:

From the IDE
Set App.AppHost as the startup project and click Run/Debug.
Aspire will start the API and any required infrastructure (e.g. Postgres) for you.

Or from the command line

```bash
dotnet run --project App.AppHost/App.AppHost.csproj
```

---

## Frontend Setup

From the repo root: 

```bash
cd frontend

# Install dependencies
npm install

# Start the dev server
npm run dev
```
By default Vite will start on something like http://localhost:5173.
The frontend is configured to call the API hosted by App.Api (via Aspire). If you change API URLs/ports, adjust the frontend config accordingly (see frontend/src/api/index.ts).


---

## Notes / Future Improvements

* Add more validation especially on API requests
* Add more filtering
* Add auth
* containerize frontend as part of the aspire host or docker-compose stack
* Can use event sourcing

