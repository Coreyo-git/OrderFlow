# CLAUDE.md

## Project Overview

OrderFlow is an event-driven microservices learning project. The owner is actively learning DDD, clean architecture, Kafka, Kubernetes, and related backend patterns by building this system incrementally.

See `event_driven_microservices_learning_project.md` for the full roadmap and current progress.

## Build & Test

```bash
# Build the solution
dotnet build

# Run all tests
dotnet test 

# Run with Docker
docker compose up -d     # CustomerService on localhost:5100, PostgreSQL on 5432
docker compose down
```

All projects including tests are in the solution file — `dotnet build` and `dotnet test` work from the repo root.

## Solution Layout

```
src/
  SharedKernel/                    -> Shared value objects (Email, PhoneNumber)
  CustomerService/                 -> ASP.NET Core Web API (port 5100 via Docker)
  CustomerService.Domain/          -> Aggregates, value objects, exceptions, interfaces
  CustomerService.Application/     -> Application services, DTOs, validators
  CustomerService.Infrastructure/  -> EF Core DbContext, repositories, migrations, configs
  OrderService/                    -> ASP.NET Core Web API (mirrors CustomerService's layout)
  OrderService.Domain/             -> Aggregates, value objects, domain events, interfaces
  OrderService.Application/        -> Application services, DTOs, validators, event handlers
  OrderService.Infrastructure/     -> EF Core DbContext, repositories, migrations, configs
tests/
  CustomerService.Domain.Tests/       -> xUnit + FluentAssertions
  OrderService.Domain.Tests/          -> xUnit + FluentAssertions
  CustomerService.API.IntegrationTests/ -> xUnit + WebApplicationFactory + Testcontainers.PostgreSql
  OrderService.API.IntegrationTests/    -> xUnit + WebApplicationFactory + Testcontainers.PostgreSql
```

Each bounded context follows clean architecture: Domain -> Application -> Infrastructure -> API.

## Key Conventions

- .NET 10, C# with nullable enabled
- PostgreSQL via EF Core (Npgsql), snake_case column naming
- Value objects use static `From()` factory methods with validation, throw `DomainException` on invalid input
- Aggregates use static `Create()` factory methods
- FluentValidation for request DTOs at the API boundary
- Test classes use nested classes to group by method (e.g. `CustomerTests+Create`, `CustomerTests+Deactivate`)
- Commit messages are short imperative sentences, commits are signed with `-S`

## Dependency Graph

```
CustomerService -> CustomerService.Application -> CustomerService.Domain -> SharedKernel
                -> CustomerService.Infrastructure -> CustomerService.Domain -> SharedKernel

OrderService -> OrderService.Application -> OrderService.Domain -> SharedKernel
             -> OrderService.Infrastructure -> OrderService.Domain -> SharedKernel
```

## Teaching & Learning Guidelines

This is a learning project. The owner is building real skills, not just shipping features. The goal is understanding, not velocity.

### Teach actively

- **Explain the "why" before anything else.** When introducing a new pattern, concept, or architectural decision, explain the reasoning and trade-offs first.
- **Name the patterns.** When code follows a known pattern (repository, factory method, aggregate root, outbox, saga, etc.), name it explicitly so the owner builds vocabulary.
- **Point out DDD principles in context.** Call out when something protects an invariant, enforces ubiquitous language, or respects aggregate boundaries — and explain why that matters.
- **Connect work to the roadmap.** Reference which phase a task belongs to and what it unlocks next.
- **Make trade-offs explicit.** When there are multiple valid approaches, lay out the options with pros/cons instead of picking one silently.

### Keep scope in check

- **Flag when something is over-engineered.** If a simpler approach teaches the same concept, say so.
