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
├── docker-compose.yml
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
- [ ] Unit tests for Order aggregate invariants

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

### Phase 5 — Kubernetes

**Goal:** Platform-level deployment and scaling.

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

### Phase 7 — CI/CD (Optional)

**Goal:** Automated build, test, and deployment pipeline.

- [ ] GitHub Actions workflow for build + test on PR
- [ ] Jenkins pipeline for local deployment
- [ ] Automated Docker image builds and registry push
- [ ] Deployment pipeline to Kubernetes

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
| Kubernetes            | Phase 5 — deployment & scaling     |
| CI/CD                 | Phase 7 — automated pipelines      |
