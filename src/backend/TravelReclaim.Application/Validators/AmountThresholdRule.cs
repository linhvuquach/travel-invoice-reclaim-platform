using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;

namespace TravelReclaim.Application.Validators;

public class AmountThresholdRule : IValidationRule
{
    private const decimal MinAmount = 0m;
    private const decimal MaxAmount = 1_000_000m;
    public string RuleName => nameof(AmountThresholdRule);

    public Task<ValidationRuleResult> ValidateAsync(Invoice invoice, CancellationToken ct = default)
    {
        var passed = invoice.TotalAmount > MinAmount && invoice.TotalAmount <= MaxAmount;
        var result = new ValidationRuleResult
        {
            RuleName = RuleName,
            Passed = passed,
            FailureReason = passed
            ? null
            : $"Total amount {invoice.TotalAmount} must be greater than {MinAmount} and at most {MaxAmount}"
        };

        return Task.FromResult(result);
    }
}