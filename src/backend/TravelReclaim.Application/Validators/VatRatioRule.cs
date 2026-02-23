using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;

namespace TravelReclaim.Application.Validators;

public class VatRatioRule : IValidationRule
{
    private const decimal MaxVatPercentage = 25m;
    public string RuleName => nameof(VatRatioRule);

    public Task<ValidationRuleResult> ValidateAsync(Invoice invoice, CancellationToken ct = default)
    {
        if (invoice.TotalAmount == 0)
        {
            return Task.FromResult(new ValidationRuleResult
            {
                RuleName = RuleName,
                Passed = false,
                FailureReason = "Cannot validate VAT ratio when total amount is zero."
            });
        }

        var vatPercentage = invoice.VatAmount / invoice.TotalAmount * 100;
        var passed = vatPercentage <= MaxVatPercentage;

        return Task.FromResult(new ValidationRuleResult
        {
            RuleName = RuleName,
            Passed = passed,
            FailureReason = passed
                ? null
                : $"VAT ratio {vatPercentage:F2}% exceeds maximum allowed {MaxVatPercentage}%."
        });
    }
}
