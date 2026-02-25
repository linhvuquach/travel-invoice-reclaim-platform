# ADR-001: Event-based audit communication via RabbitMQ + MassTransit

**Date**: 2026-02-25
**Status**: Accepted
**Deciders**: Engineering team

---

## Context

Phase 1 CQRS handlers (`CreateInvoiceHandler`, `ProcessReclaimHandler`, `ApproveReclaimHandler`, `RejectReclaimHandler`) called `IAuditService.LogEventAsync` directly after each state-changing operation. This created tight coupling between business logic and the audit/observability concern:

- Handlers had to know about `IAuditService` even though auditing is an orthogonal concern.
- A slow or unavailable audit store would block command execution.
- Adding new subscribers to domain events (e.g., notifications, analytics) required modifying existing handlers.

## Decision

Replace direct `IAuditService.LogEventAsync` calls in CQRS handlers with domain event publishing through an `IEventBus` abstraction. Handlers publish events; a dedicated event handler (`InvoiceCreatedEventHandler`, etc.) writes to the audit store asynchronously.

Use **RabbitMQ** (via **MassTransit 8.x**) as the broker rather than starting with an in-memory bus — the production topology is established from day one.

## Architecture

```
CQRS Handler
  └─► IEventBus.PublishAsync(event)          [Application layer]
        └─► RabbitMqEventBus                 [Infrastructure layer]
              └─► MassTransit IPublishEndpoint
                    └─► RabbitMQ exchange
                          └─► MassTransit Consumer (e.g. InvoiceCreatedConsumer)
                                └─► IEventHandler<TEvent>.HandleAsync
                                      └─► IAuditService.LogEventAsync

ProcessReclaimHandler (exception)
  └─► IAuditService.LogValidationAsync       [direct call — kept intentionally]
```

## Domain events introduced

| Event                   | Published by            | Handled by                     |
| ----------------------- | ----------------------- | ------------------------------ |
| `InvoiceCreatedEvent`   | `CreateInvoiceHandler`  | `InvoiceCreatedEventHandler`   |
| `ReclaimProcessedEvent` | `ProcessReclaimHandler` | `ReclaimProcessedEventHandler` |
| `ReclaimApprovedEvent`  | `ApproveReclaimHandler` | `ReclaimApprovedEventHandler`  |
| `ReclaimRejectedEvent`  | `RejectReclaimHandler`  | `ReclaimRejectedEventHandler`  |

## Exception: `LogValidationAsync` remains a direct call

`ProcessReclaimHandler` keeps a direct call to `IAuditService.LogValidationAsync` because:

1. It captures per-rule `ExecutionTimeMs` measured inside the handler scope.
2. Validation telemetry is operational data (timing), not a domain event.
3. Moving it to an event would require serialising timing state that has no meaning outside the handler.

## Consequences

**Positive**

- Handlers are decoupled from auditing; adding new event consumers requires zero changes to existing handlers.
- RabbitMQ topology is established early, avoiding a later rip-and-replace migration.
- `IEventBus` is easily mocked in unit tests; integration tests replace MassTransit hosted services with a no-op mock.

**Negative / trade-offs**

- At-least-once delivery: consumers must be idempotent (acceptable for audit writes by correlation ID).
- Local development requires Docker Compose with RabbitMQ running (`docker compose up -d rabbitmq`).
- Integration tests stub `IEventBus` — event handler logic is only covered by unit tests.

## Resilience

- MassTransit provides automatic retry, dead-letter queues, and outbox pattern when needed

## Alternatives considered

| Option                                   | Why rejected                                                                                                             |
| ---------------------------------------- | ------------------------------------------------------------------------------------------------------------------------ |
| Keep direct `LogEventAsync` calls        | Tight coupling; every new observer requires handler changes                                                              |
| In-memory event bus first, migrate later | Added a migration step with no observable benefit; RabbitMQ added minimal complexity via MassTransit                     |
| MediatR notifications                    | Added a third-party dependency to the Application layer with no additional benefit over the custom `IEventBus` interface |
