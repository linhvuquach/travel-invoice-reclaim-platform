using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Application.Events.Handlers;

public class InvoiceCreateEventHandler(IAuditService auditService) : IEventHandler<InvoiceCreatedEvent>
{
    public Task HandleAsync(InvoiceCreatedEvent @event, CancellationToken ct = default) =>
    auditService.LogEventAsync(new AuditEvent
    {
        EntityId = @event.InvoiceId,
        EntityType = "Invoice",
        Action = "Created",
        PerformedBy = @event.PerformedBy,
        Timestamp = @event.Timestamp
    }, ct);
}