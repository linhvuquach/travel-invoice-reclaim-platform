using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Exceptions;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Reclaims.Commands;

public class ApproveReclaimHandler(
    IReclaimRepository reclaimRepository,
    IInvoiceRepository invoiceRepository,
    IAuditService auditService
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

        await auditService.LogEventAsync(new AuditEvent
        {
            EntityId = reclaim.Id,
            EntityType = "Reclaim",
            Action = "Approved",
            PerformedBy = command.ProcessedBy
        });

        var saved = await reclaimRepository.GetByIdWithInvoiceAsync(reclaim.Id, ct);
        return ReclaimMapper.ToResponse(saved!);
    }
}