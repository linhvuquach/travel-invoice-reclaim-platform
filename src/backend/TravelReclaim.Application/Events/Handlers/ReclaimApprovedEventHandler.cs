using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Application.Events.Handlers;

public class ReclaimApprovedEventHandler(IAuditService auditService) : IEventHandler<ReclaimApprovedEvent>
{
    public Task HandleAsync(ReclaimApprovedEvent @event, CancellationToken ct = default) =>
        auditService.LogEventAsync(new AuditEvent
        {
            EntityId = @event.ReclaimId,
            EntityType = "Reclaim",
            Action = "Approved",
            PerformedBy = @event.ApprovedBy,
            Timestamp = @event.Timestamp
        }, ct);
}
