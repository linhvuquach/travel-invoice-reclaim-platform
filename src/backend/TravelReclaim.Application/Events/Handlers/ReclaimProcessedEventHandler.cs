using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Application.Events.Handlers;

public class ReclaimProcessedEventHandler(IAuditService auditService) : IEventHandler<ReclaimProcessedEvent>
{
    public Task HandleAsync(ReclaimProcessedEvent @event, CancellationToken ct = default)
    {
        var auditEvent = new AuditEvent
        {
            EntityId = @event.ReclaimId ?? @event.InvoiceId,
            EntityType = @event.Outcome == "Created" ? "Reclaim" : "Invoice",
            Action = @event.Outcome == "Created" ? "Created" : "Rejected",
            PerformedBy = @event.PerformedBy,
            Timestamp = @event.Timestamp
        };

        if (@event.RejectionReason is not null)
            auditEvent.Metadata = new Dictionary<string, object> { ["rejectionReason"] = @event.RejectionReason };

        return auditService.LogEventAsync(auditEvent, ct);
    }
}
