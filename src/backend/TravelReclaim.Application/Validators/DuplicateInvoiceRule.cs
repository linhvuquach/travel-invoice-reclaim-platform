using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Validators;

public class DuplicateInvoiceRule(IReclaimRepository reclaimRepository) : IValidationRule
{
    public string RuleName => nameof(DuplicateInvoiceRule);

    public async Task<ValidationRuleResult> ValidateAsync(Invoice invoice, CancellationToken ct = default)
    {
        var existingReclaim = await reclaimRepository.GetByInvoiceIdAsync(invoice.Id, ct);
        var passed = existingReclaim is null;

        return new ValidationRuleResult
        {
            RuleName = RuleName,
            Passed = passed,
            FailureReason = passed
                ? null
                : $"Invoice '{invoice.InvoiceNumber}' already has an existing reclaim."
        };
    }
}
