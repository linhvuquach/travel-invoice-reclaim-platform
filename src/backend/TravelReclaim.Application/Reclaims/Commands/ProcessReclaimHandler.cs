using System.Diagnostics;
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
        IEnumerable<IValidationRule> validationRules,
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
            var totalStopWatch = Stopwatch.StartNew();
            List<ValidationRuleResult> validationResults = await ValidateInvoice(invoice, ct);

            totalStopWatch.Stop();

            var allPassed = validationResults.All(r => r.Passed);

            // Log validation event to MongoDB
            await auditService.LogValidationAsync(new ValidationEvent
            {
                InvoiceId = invoice.Id,
                HotleName = invoice.HotelName,
                Results = validationResults,
                OverallOutcome = allPassed ? "Passed" : "Failed",
                TotalExecutionTimeMs = totalStopWatch.ElapsedMilliseconds,
                TriggedBy = "system"
            });

            ReclaimResponse? reclaimResponse = null;

            if (allPassed)
                reclaimResponse = await HandlePassedValidationRule(invoice, reclaimResponse, ct);
            else
                await HandleFailedValidationRule(invoice, validationResults, ct);

            var validationResultDtos = validationResults.Select(r => new ValidationRuleResultDto(r.RuleName, r.Passed, r.FailureReason)).ToList();
            return new ProcessReclaimResponse(reclaimResponse, allPassed, validationResultDtos);
        }

        private async Task<List<ValidationRuleResult>> ValidateInvoice(Invoice invoice, CancellationToken ct)
        {
            var validationResults = new List<ValidationRuleResult>();

            foreach (var rule in validationRules)
            {
                var ruleStopWatch = Stopwatch.StartNew();
                var result = await rule.ValidateAsync(invoice, ct);
                ruleStopWatch.Stop();
                result.ExecutionTimeMs = ruleStopWatch.ElapsedMilliseconds;
                validationResults.Add(result);
            }

            return validationResults;
        }

        private async Task<ReclaimResponse> HandlePassedValidationRule(Invoice invoice, ReclaimResponse reclaimResponse, CancellationToken ct)
        {
            var reclaim = Reclaim.Create(invoice.Id, invoice.VatAmount);
            await reclaimRepository.AddAsync(reclaim, ct);

            invoice.MarkAsReclaimPending();
            await invoiceRepository.UpdateAsync(invoice, ct);

            // Re-fetch with navigation for mapping
            var savedReclaim = await reclaimRepository.GetByIdWithInvoiceAsync(reclaim.Id, ct);
            reclaimResponse = ReclaimMapper.ToResponse(savedReclaim!);

            await auditService.LogEventAsync(new AuditEvent
            {
                EntityId = reclaim.Id,
                EntityType = "Reclaim",
                Action = "Created",
                PerformedBy = "system"
            }, ct);

            return reclaimResponse;
        }

        private async Task HandleFailedValidationRule(Invoice invoice, List<ValidationRuleResult> validationResults, CancellationToken ct)
        {
            var failedReasons = string.Join("; ", validationResults.Where(r => !r.Passed).Select(r => r.FailureReason));
            invoice.MarkAsRejected(failedReasons);
            await invoiceRepository.UpdateAsync(invoice, ct);

            await auditService.LogEventAsync(new AuditEvent
            {
                EntityId = invoice.Id,
                EntityType = "Invoice",
                Action = "Rejected",
                PerformedBy = "system",
                Metadata = new Dictionary<string, object> { ["rejectionReason"] = failedReasons }
            }, ct);
        }
    }
}