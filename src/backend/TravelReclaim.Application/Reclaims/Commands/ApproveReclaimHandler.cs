using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Exceptions;
using TravelReclaim.Domain.Interfaces;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Application.Reclaims.Commands;

public class ApproveReclaimHandler(
    IReclaimRepository reclaimRepository,
    IInvoiceRepository invoiceRepository,
    IEventBus eventBus
) : ICommandHandler<ApproveReclaimCommand, ReclaimResponse>
{
    public async Task<ReclaimResponse> HandleAsync(ApproveReclaimCommand command, CancellationToken ct = default)
    {
        var reclaim = await reclaimRepository.GetByIdWithInvoiceAsync(command.ReclaimId, ct)
            ?? throw new EntityNotFoundException("Reclaim", command.ReclaimId);

        reclaim.Approve(command.ProcessedBy);
        await reclaimRepository.UpdateAsync(reclaim, ct);

        var invoice = await invoiceRepository.GetByIdAsync(reclaim.InvoiceId, ct);
        invoice!.MarkAsReclaimed();
        await invoiceRepository.UpdateAsync(invoice, ct);

        await eventBus.PublishAsync(new ReclaimApprovedEvent(
            ReclaimId: reclaim.Id,
            ApprovedBy: command.ProcessedBy,
            Timestamp: DateTime.UtcNow), ct);

        var saved = await reclaimRepository.GetByIdWithInvoiceAsync(reclaim.Id, ct);
        return ReclaimMapper.ToResponse(saved!);
    }
}