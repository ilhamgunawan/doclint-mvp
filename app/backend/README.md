# DocLint Backend

REST API for the DocLint PDF document format validator.

## Tech Stack

- **.NET 10** + **ASP.NET Core**
- **EF Core** + **SQLite**
- **AutoMapper**
- **OpenAPI** / **Scalar UI**

## Architecture

Clean Architecture with 4 projects:

| Project | Layer | Responsibility |
|---|---|---|
| `DocLint.Domain` | Core | Entities, enums |
| `DocLint.Application` | Use Cases | DTOs, services, interfaces, AutoMapper |
| `DocLint.Infrastructure` | Persistence | EF Core DbContext, repositories |
| `DocLint.WebAPI` | Presentation | Controllers, middleware, DI wiring |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Run

```bash
dotnet run --project src/DocLint.WebAPI
```

## API Endpoints

| Method | Path | Description |
|---|---|---|
| `GET` | `/healthz` | Health check |
| `POST` | `/api/v1/documents/lint` | Lint a PDF document |

## OpenAPI Reference

In development mode, visit `/scalar` for the interactive API reference.
