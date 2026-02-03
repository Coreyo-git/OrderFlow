# OrderFlow

An event-driven microservices platform built with .NET 9, designed as a hands-on learning project for Domain-Driven Design, Kafka, Kubernetes, and production-grade backend architecture.

## What is this?

OrderFlow is an Order Management system where each microservice is a DDD bounded context that owns its domain model and database. Services communicate asynchronously through Kafka. The project is built incrementally — starting with domain modelling fundamentals and progressively layering in messaging, reliability patterns, containerisation, and orchestration.

This isn't a tutorial follow-along. It's a from-scratch implementation where each architectural decision is made deliberately as part of the learning process.

## Architecture

```
[ React Frontend ]
        |
   [ API Gateway ]
        |
----------------------------------------------
| CustomerService | OrderService | ...future |
----------------------------------------------
        |               |
      Kafka           Kafka
        |               |
----------------------------------------------
| PaymentService | InventoryService | Notify |
----------------------------------------------
```

Each service follows clean architecture: **Domain → Application → Infrastructure → API**.

## Current State

**CustomerService** is the first fully implemented bounded context:

- `Customer` aggregate with factory method, activate/deactivate, and email change cooldown business rule
- Value objects with validation and encapsulation (`CustomerId`, `CustomerName`, `Email`, `PhoneNumber`)
- Clean architecture across four projects (Domain, Application, Infrastructure, API)
- SharedKernel for cross-service value objects
- EF Core + PostgreSQL persistence
- REST API with FluentValidation
- Docker Compose for local development
- 61 passing unit tests

## Tech Stack

| Layer           | Technology                          |
|-----------------|-------------------------------------|
| Language        | C# / .NET 9                        |
| API             | ASP.NET Core, FluentValidation      |
| Persistence     | EF Core, PostgreSQL                 |
| Messaging       | Apache Kafka (planned)              |
| Testing         | xUnit, FluentAssertions             |
| Containers      | Docker, Docker Compose              |
| Orchestration   | Kubernetes (planned)                |
| Observability   | Serilog, OpenTelemetry, Grafana (planned) |
| CI/CD           | GitHub Actions, Jenkins (planned)   |

## Project Structure

```
OrderFlow/
├── src/
│   ├── SharedKernel/                  # Shared value objects
│   ├── CustomerService/               # Web API entry point
│   ├── CustomerService.Domain/        # Aggregates, value objects, interfaces
│   ├── CustomerService.Application/   # Application services, DTOs, validators
│   ├── CustomerService.Infrastructure/# EF Core, repositories, migrations
│   └── OrderService/                  # Stub — next up
├── tests/
│   └── CustomerService.Domain.Tests/
├── docker-compose.yml
└── OrderFlow.sln
```

## Running Locally

```bash
docker compose up -d
```

This starts CustomerService on `localhost:5100` with a PostgreSQL database.

## Running Tests

```bash
dotnet test
```

## Roadmap

The project is built in phases. See [the full roadmap](event_driven_microservices_learning_project.md) for detailed checklists.

| Phase | Focus                        | Status      |
|-------|------------------------------|-------------|
| 1     | Domain models & clean architecture | In progress |
| 2     | Domain events                | Planned     |
| 3     | Kafka & microservices        | Planned     |
| 4     | Reliability patterns         | Planned     |
| 5     | Kubernetes                   | Planned     |
| 6     | Observability                | Planned     |
| 7     | CI/CD                        | Planned     |
