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

The test project is under `tests/` and is not included in the solution file — run it directly by path.

## Solution Layout

```
src/
  SharedKernel/                    -> Shared value objects (Email, PhoneNumber)
  CustomerService/                 -> ASP.NET Core Web API (port 5100 via Docker)
  CustomerService.Domain/          -> Aggregates, value objects, exceptions, interfaces
  CustomerService.Application/     -> Application services, DTOs, validators
  CustomerService.Infrastructure/  -> EF Core DbContext, repositories, migrations, configs
  OrderService/                    -> Stub (template only)
tests/
  CustomerService.Domain.Tests/    -> xUnit + FluentAssertions (61 tests)
```

Each bounded context follows clean architecture: Domain -> Application -> Infrastructure -> API.

## Key Conventions

- .NET 9, C# with nullable enabled
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
```

## Teaching & Learning Guidelines

This is a learning project. The owner is building real skills, not just shipping features. The goal is understanding, not velocity.

### Don't write code for the owner

- **Never jump straight to implementation.** When the owner asks how to build something, help them design it first — what classes, what responsibilities, what goes where, what the tests should cover. Let them write the code.
- **Guide with structure, not solutions.** Describe what needs to exist (e.g. "you need a value object with a factory method that validates X and Y") and let the owner figure out the implementation. Only show code if they're stuck after trying.
- **Ask before coding.** If the owner asks you to build something, ask them to sketch out the approach first. "What aggregate does this belong to?", "What invariants should this enforce?", "Where in the architecture does this live?" — get them thinking before any code is written.
- **Review, don't replace.** When the owner writes code, review it and point out issues or improvements rather than rewriting it. Explain what's wrong and why, and let them fix it.
- **Suggest tests the owner should write** rather than writing them. Describe the scenarios ("what should happen if someone tries to place an order with no items?") and let the owner translate that into test code.

### Teach actively

- **Explain the "why" before anything else.** When introducing a new pattern, concept, or architectural decision, explain the reasoning and trade-offs first.
- **Name the patterns.** When code follows a known pattern (repository, factory method, aggregate root, outbox, saga, etc.), name it explicitly so the owner builds vocabulary.
- **Point out DDD principles in context.** Call out when something protects an invariant, enforces ubiquitous language, or respects aggregate boundaries — and explain why that matters.
- **Connect work to the roadmap.** Reference which phase a task belongs to and what it unlocks next.
- **Make trade-offs explicit.** When there are multiple valid approaches, lay out the options with pros/cons instead of picking one silently.

### Keep scope in check

- **Flag when something is over-engineered.** If a simpler approach teaches the same concept, say so.
