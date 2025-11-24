# 🎮 Media Ranker(ASP.NET Core + PostgreSQL)

A simple movie and review application built using ASP.NET Core Web API, leveraging Entity Framework Core and PostgreSQL as the database. This project is dedicated to learning C#, EF Core fundamentals, and building a robust REST API.

---
## Configuration & Setup
### Prerequisites
* .NET 8
* Node.js 18+ & npm
* Angular CLI (install globally: npm install -g @angular/cli)
* PostgreSQL
* Visual Studio / VS Code
* `dotnet-ef` CLI
#### 1. Clone the Repository

```bash
git clone https://github.com/twoj-login/filmweb-clone.git
cd filmweb-clone
```

#### 2. Configure Database Connection

Update the connection string in appsettings.json, ensuring the Username and Password match your local PostgreSQL setup:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=FilmwebDb;Username=postgres;Password=TwojeHaslo"
}
```
---

## EF Core Migrations

### Install EF Core Tools (If necessary):

```bash
dotnet tool install --global dotnet-ef
```

### Create and Apply Migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
---
## 🚀 technologies

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* PostgreSQL
* JWT Authentication
* REST API
* Angular
---
---

## Features
* User registration & login with JWT authentication
* View, filter with predicate anad dynamic sort for data
* User can add review to entities
---

## Future Development
*  Frontend (Angular)
*  Recommendations
*  docker
*  clean architecture with vertical slice

---
