using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Exceptions;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Reclaims.Commands;

public class RejectReclaimHandler(
    IReclaimRepository reclaimRepository,
    IAuditService auditService
) : ICommandHandler<RejectReclaimCommand, ReclaimResponse>
{
    public async Task<ReclaimResponse> HandleAsync(RejectReclaimCommand command, CancellationToken ct = default)
    {
        var reclaim = await reclaimRepository.GetByIdWithInvoiceAsync(command.ReclaimId)
            ?? throw new EntityNotFoundException("Reclaim", command.ReclaimId);

        reclaim.Reject(command.Reason, command.ProcessedBy);
        await reclaimRepository.UpdateAsync(reclaim, ct);

        await auditService.LogEventAsync(new AuditEvent
        {
            EntityId = reclaim.Id,
            EntityType = "Reclaim",
            Action = "Rejected",
            PerformedBy = command.ProcessedBy,
            Metadata = new Dictionary<string, object> { ["rejectReason"] = command.Reason }
        }, ct);

        var saved = await reclaimRepository.GetByIdWithInvoiceAsync(reclaim.Id, ct);
        return ReclaimMapper.ToResponse(saved!);
    }
}