using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Application.Events.Handlers;

public class ReclaimRejectedEventHandler(IAuditService auditService) : IEventHandler<ReclaimRejectedEvent>
{
    public Task HandleAsync(ReclaimRejectedEvent @event, CancellationToken ct = default) =>
        auditService.LogEventAsync(new AuditEvent
        {
            EntityId = @event.ReclaimId,
            EntityType = "Reclaim",
            Action = "Rejected",
            PerformedBy = @event.RejectedBy,
            Timestamp = @event.Timestamp,
            Metadata = new Dictionary<string, object> { ["rejectionReason"] = @event.Reason }
        }, ct);
}
