using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Exceptions;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Reclaims.Commands
{
    public class ProcessReclaimHandler(
        IInvoiceRepository invoiceRepository,
        IReclaimRepository reclaimRepository,
        IAuditService auditService
    ) : ICommandHandler<ProcessReclaimCommand, ProcessReclaimResponse>
    {
        public async Task<ProcessReclaimResponse> HandleAsync(ProcessReclaimCommand command, CancellationToken ct = default)
        {
            var invoice = await invoiceRepository.GetByIdAsync(command.InvoiceId, ct)
                ?? throw new EntityNotFoundException("Invoice", command.InvoiceId);

            if (invoice.Status != InvoiceStatus.Submitted)
                throw new BusinessRuleException(
                    $"Invoice must be in '{InvoiceStatus.Submitted}' status to process reclaim. Current status: '{invoice.Status}'.");

            // Execute all valadation rules and measure time
            var results = new List<ValidationRuleResult>();

            // Log validation event to MongoDB

            var resultDtos = results.Select(r => new ValidationRuleResultDto(r.RuleName, r.Passed, r.FailureReason)).ToList();
            return new ProcessReclaimResponse(null, false, resultDtos);
        }
    }
}