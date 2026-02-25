using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Exceptions;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Application.Reclaims.Commands;

public class RejectReclaimHandler(
    IReclaimRepository reclaimRepository,
    IEventBus eventBus
) : ICommandHandler<RejectReclaimCommand, ReclaimResponse>
{
    public async Task<ReclaimResponse> HandleAsync(RejectReclaimCommand command, CancellationToken ct = default)
    {
        var reclaim = await reclaimRepository.GetByIdWithInvoiceAsync(command.ReclaimId)
            ?? throw new EntityNotFoundException("Reclaim", command.ReclaimId);

        reclaim.Reject(command.Reason, command.ProcessedBy);
        await reclaimRepository.UpdateAsync(reclaim, ct);

        await eventBus.PublishAsync(new ReclaimRejectedEvent(
            ReclaimId: reclaim.Id,
            Reason: command.Reason,
            RejectedBy: command.ProcessedBy,
            Timestamp: DateTime.UtcNow), ct);

        var saved = await reclaimRepository.GetByIdWithInvoiceAsync(reclaim.Id, ct);
        return ReclaimMapper.ToResponse(saved!);
    }
}