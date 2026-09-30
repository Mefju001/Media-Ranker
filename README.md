# Media Ranker (ASP.NET Core + Angular + PostgreSQL)

**Media Ranker** is a full-stack web application inspired by platforms like Filmweb or IMDb. It allows users to catalog, rate, filter, and review various types of media, including movies, TV series, and video games.

The project is built with **ASP.NET Core 8 Web API** leveraging **CQRS (MediatR)** architecture and domain-driven practices, paired with an **Angular** frontend and a **PostgreSQL** database. The entire environment is fully containerized with **Docker** and features an automated **CI/CD pipeline (GitHub Actions)**.

---

## Quick Start (Docker)

The easiest and recommended way to run the entire application (Database, API, and Frontend) is using Docker Compose:

1. Clone the repository:
   git clone [https://github.com/Mefju001/Media-Ranker.git](https://github.com/Mefju001/Media-Ranker.git)
   cd Media-Ranker

2. Create an environment file:
   Copy `.env.example` to `.env` and fill in your values (or use the defaults):
   cp .env.example .env

   Example `.env` content:
   DB_USER=postgres
   DB_PASSWORD=your_secure_password!
   DB_NAME=MovieDb
   JWT_SECRET_KEY=YourSuperSecretKeyThatIsAtLeast32BytesLong!
   ASPNETCORE_ENVIRONMENT=Development

3. Start the application stack:
   docker compose up -d

Once running, access the services at:
* **Frontend (Angular / Nginx):** http://localhost
* **API / Swagger UI:** http://localhost:5000/swagger/index.html

---

## Tech Stack

* **Backend:** ASP.NET Core 8 Web API, Entity Framework Core 8, MediatR (CQRS), FluentValidation, JWT Authentication
* **Frontend:** Angular 17+, Nginx
* **Database:** PostgreSQL
* **Testing:** MSTest, SQLite In-Memory
* **DevOps & Infrastructure:** Docker, Docker Compose, GitHub Actions (CI/CD)

---

## Features & Architecture

* **Authentication & Authorization:** Secure user registration and login using JWT tokens.
* **Media Catalog:** Browse, dynamically sort, and filter games, movies, and TV series with custom predicates.
* **User Interactions & Reviews:** Rate entities, manage interaction statuses (e.g., Planned, In Progress, Completed), and write reviews.
* **CQRS & MediatR Pipeline:** Clean Command/Query Responsibility Segregation with open behaviors for validation (ValidationBehaviour), logging (LoggingBehaviour), and database transactions (TransactionBehaviour).
* **Automated Testing:** Integration and unit tests built with MSTest and SQLite In-Memory for fast and reliable domain logic verification.

---

## Local Development Setup

To run and debug the project without Docker:

### Prerequisites:
* .NET 8 SDK
* Node.js 18+ & npm
* PostgreSQL

### 1. Configure Secrets (Visual Studio User Secrets / .NET CLI)
Local configuration relies on **.NET User Secrets** to keep sensitive data out of source control. 

Right-click the `Api` project in Visual Studio -> **Manage User Secrets** and configure the required settings:

{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=yourPassword"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32BytesLong!",
    "Key2": "YourRefreshTokenSecretKeyAtLeast32BytesLong!"
  }
}

### 2. Apply EF Core Migrations
dotnet ef database update --project Api

### 3. Run Backend (.NET API)
dotnet run --project Api

### 4. Run Frontend (Angular)
cd angular-movieFrontend
npm install
ng serve

---

## Testing

The repository includes a suite of MSTest integration and unit tests. Run them using:

dotnet test

---

## Future Development

* Enhanced user profiles with generated statistics (e.g., total watch/play time).
