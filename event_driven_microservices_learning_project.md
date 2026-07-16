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
| OrderService       | Ordering        | Stub        |
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
│   └── OrderService/                  # Stub — future implementation
├── tests/
│   └── CustomerService.Domain.Tests/  # 61 unit tests (xUnit + FluentAssertions)
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

- [ ] In-process domain event dispatch (MediatR or custom `IEventDispatcher`) - learn what MediatR is and why I would use it
- [ ] Domain events: `OrderPlaced`, `OrderConfirmed`, `OrderCancelled`
- [ ] Customer domain events: `CustomerDeactivated` (notify other contexts)
- [ ] Event handlers in the application layer

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
