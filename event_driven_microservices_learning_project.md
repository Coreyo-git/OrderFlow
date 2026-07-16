# OrderFlow — Event-Driven Microservices Learning Project

## Overview

A **production-style event-driven Order Management platform** for building practical skills in:

- Domain-Driven Design (DDD)
- Event-driven microservices
- Apache Kafka
- .NET 9 (ASP.NET Core)
- React
- Docker & Docker Compose
- Kubernetes
- Azure (AKS, Container Registry, Key Vault) via Terraform
- CI/CD with GitHub Actions

Each microservice represents a **bounded context**, communicates **asynchronously via Kafka**, and owns its **domain model and database**.

---

## High-Level Architecture

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

---

## Bounded Contexts

| Microservice       | Bounded Context | Status      |
|--------------------|-----------------|-------------|
| CustomerService    | Customers       | In progress |
| OrderService       | Ordering        | In progress |
| PaymentService     | Payments        | Planned     |
| InventoryService   | Inventory       | Planned     |
| NotificationService| Messaging       | Planned     |

---

## Solution Structure

```
OrderFlow/
├── src/
│   ├── SharedKernel/                  # Shared value objects (Email, PhoneNumber)
│   ├── CustomerService/               # ASP.NET Core Web API (entry point)
│   │   ├── API/Controllers/
│   │   ├── Filters/
│   │   ├── Dockerfile
│   │   └── Program.cs
│   ├── CustomerService.Domain/        # Aggregates, value objects, domain exceptions
│   │   ├── Aggregates/
│   │   ├── ValueObjects/
│   │   ├── Exceptions/
│   │   └── Interfaces/
│   ├── CustomerService.Application/   # Application services, DTOs, validators
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Services/
│   │   └── Validators/
│   ├── CustomerService.Infrastructure/# EF Core, repositories, migrations
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   └── Repositories/
│   ├── OrderService/                  # ASP.NET Core Web API — mirrors CustomerService's layout
│   ├── OrderService.Domain/
│   ├── OrderService.Application/
│   └── OrderService.Infrastructure/
├── tests/
│   ├── CustomerService.Domain.Tests/
│   ├── CustomerService.API.IntegrationTests/
│   ├── OrderService.Domain.Tests/
│   └── OrderService.API.IntegrationTests/
├── infra/
│   └── terraform/                     # Azure resource group, ACR, GitHub OIDC (scaffolded), AKS (planned)
├── docker-compose.yml
├── .env.example                       # Template for local secrets (.env is git-ignored)
└── OrderFlow.sln
```

Each service follows **clean architecture**: Domain → Application → Infrastructure → API.

---

## Database Strategy

| Service            | Database   | Rationale                              |
|--------------------|------------|----------------------------------------|
| CustomerService    | PostgreSQL | Relational data, ACID transactions     |
| OrderService       | PostgreSQL | Order state, line items                 |
| PaymentService     | PostgreSQL | Transaction history, audit requirements|
| InventoryService   | PostgreSQL | Stock levels, reservation tracking     |
| NotificationService| Redis      | Stateless, or Redis for deduplication  |

---

## Testing Strategy

| Test Type   | What to Test                                     | Tools                        |
|-------------|--------------------------------------------------|------------------------------|
| Unit        | Domain logic (aggregates, value objects, invariants) | xUnit, FluentAssertions      |
| Integration | Kafka consumers, database repositories           | Testcontainers               |
| End-to-End  | Full user flows across services                  | Docker Compose test harness  |

---

## Build Roadmap

### Phase 1 — Domain First (DDD Fundamentals)

**Goal:** Build expressive domain models with clean architecture, persistence, and containerisation.

- [x] CustomerService aggregate (`Customer`) with factory method, activate/deactivate, email change cooldown rule
- [x] Value objects: `CustomerId`, `CustomerName`, `Email`, `PhoneNumber`
- [x] Clean architecture layers (Domain → Application → Infrastructure → API)
- [x] SharedKernel for cross-service value objects
- [x] EF Core + PostgreSQL persistence with migrations - Consider InMem before infra to setup domain first
- [x] Docker Compose (CustomerService + PostgreSQL)
- [x] Domain unit tests passing (xUnit + FluentAssertions)
- [x] REST API with FluentValidation
- [x] `Address` value object on Customer aggregate
- [x] OrderService domain model (`Order` aggregate with states: Draft → Placed → Confirmed → Completed → Cancelled)
- [x] Order value objects: `OrderId`, `Money`, `OrderLine`
- [x] Order state machine with invariant enforcement
- [x] Unit tests for Order aggregate invariants

---

### Phase 2 — Domain Events

**Goal:** Separate domain logic from side effects.

- [x] In-process domain event dispatch — custom `IDomainEventDispatcher` (`SharedKernel/Events/DomainEventDispatcher.cs`), resolving handlers via DI + reflection rather than MediatR: matches the `IDomainEventHandler<T>` shape already scaffolded, and the added machinery of MediatR (pipeline behaviors, request/response) isn't earning its keep yet for a couple of in-process events. Revisit MediatR once there are enough event types/handlers that the hand-rolled plumbing starts to hurt.
- [x] `AggregateRoot` base class (`SharedKernel/AggregateRoot.cs`) — `Customer` now derives from it; `Order` doesn't yet
- [ ] Domain events: `OrderPlaced`, `OrderConfirmed`, `OrderCancelled` — blocked on OrderService.Application/Infrastructure/API, which are still stub projects with no code
- [x] Customer domain events: `CustomerDeactivated`, raised from `Customer.Deactivate()`, dispatched after `SaveChangesAsync` commits
- [x] Order domain events: `OrderPlaced`, `OrderConfirmed`, `OrderCancelled` (only on an actual state transition, not the idempotent double-cancel no-op)
- [x] OrderService.Application/Infrastructure/API layers stood up (were empty stubs) — DTOs, validator, repository, EF Core `OrderDbContext` + migration, `OrdersController` (create/get/confirm/cancel), all wired into `docker-compose.yml`
- [x] Event handlers in the application layer — one handler per event (logs; stands in for the Phase 3 Kafka publisher)

---

### Phase 3 — Kafka & Microservices

**Goal:** Event-driven architecture with asynchronous communication.

- [ ] Introduce Kafka with Confluent .NET client
- [ ] Shared `Contracts` project for integration event schemas
- [ ] Publish integration events from domain events
- [ ] PaymentService bounded context
- [ ] Simple saga: `OrderPlaced` → `PaymentProcessed` → `OrderConfirmed`

---

### Phase 4 — Reliability & Production Patterns

**Goal:** Build resilient systems.

- [ ] InventoryService bounded context
- [ ] Outbox pattern (`OutboxMessage` table + background worker)
- [ ] Retry topics and dead-letter queues
- [ ] Idempotent consumers
- [ ] Failure simulation endpoint

---

### Phase 5 — Azure & Kubernetes

**Goal:** Platform-level deployment and scaling on Azure Kubernetes Service (AKS), with Container Apps as an interim comparison point along the way.

**Configuration & secrets**

- [x] Local dev secrets externalised: `.env` (git-ignored, per-service Postgres credentials) + `.env.example` (tracked template); no plaintext connection strings in `appsettings.*.json`
- [ ] Azure Key Vault provisioned via Terraform
- [ ] Managed identity / workload identity federation granting services `Key Vault Secrets User`
- [ ] App reads secrets via `Azure.Identity` + `Microsoft.Extensions.Configuration.AzureKeyVault` (portable between Container Apps and AKS — no CSI driver dependency)

**Azure infrastructure (Terraform)**

- [x] Resource group + Azure Container Registry (`infra/terraform/modules/acr`)
- [ ] GitHub OIDC module wired up (`infra/terraform/modules/github-oidc` is scaffolded but not yet referenced from `main.tf`) — federated GitHub Actions auth to Azure, no long-lived credentials
- [ ] AKS cluster module, workload identity enabled
- [ ] Container Apps environment (interim/comparison deployment target)

**Kubernetes**

- [ ] Deploy services to Kubernetes (Pods, Deployments, Services, ConfigMaps, Secrets)
- [ ] Scale services independently
- [ ] Health checks and readiness probes

---

### Phase 6 — Observability (Optional)

**Goal:** Understand what's happening in a distributed system.

- [ ] Structured logging with Serilog
- [ ] Correlation IDs across services
- [ ] OpenTelemetry for distributed tracing
- [ ] Prometheus metrics exporting
- [ ] Grafana dashboards
- [ ] Health checks per service

---

### Phase 7 — CI/CD

**Goal:** Automated build, test, and deployment pipeline.

- [x] GitHub Actions workflow for `dotnet build`/`dotnet test` on PR (`.github/workflows/dotnet.yml`)
- [x] GitHub Actions Terraform fmt check workflow (`.github/workflows/terraform.yml`)
- [ ] Terraform plan/apply pipeline via GitHub OIDC (no stored Azure credentials)
- [ ] Automated Docker image builds and registry push to ACR
- [ ] Deployment pipeline to AKS

---

## Core Concepts

| Concept               | Where It Appears                   |
|-----------------------|------------------------------------|
| Domain-Driven Design  | Inside each service                |
| Aggregates            | Customer, Order (future)           |
| Value Objects         | Email, PhoneNumber, CustomerId, CustomerName |
| Domain Events         | Phase 2                            |
| Kafka                 | Phase 3 — cross-service messaging  |
| Eventual Consistency  | Order → Payment → Inventory        |
| Outbox Pattern        | Phase 4 — reliable event publishing|
| Docker                | Local development                  |
| Terraform              | Phase 5 — Azure infra as code      |
| Azure Key Vault        | Phase 5 — centralised secrets      |
| Managed/Workload Identity | Phase 5 — passwordless auth to Azure |
| Kubernetes            | Phase 5 — deployment & scaling (AKS)|
| CI/CD                 | Phase 7 — automated pipelines      |
