using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;

namespace TravelReclaim.Application.Validators;

public class InvoiceAgeRule : IValidationRule
{
    private const int MaxAgeDays = 90;
    public string RuleName => nameof(InvoiceAgeRule);

    public Task<ValidationRuleResult> ValidateAsync(Invoice invoice, CancellationToken ct = default)
    {
        var age = (DateTime.UtcNow.Date - invoice.IssueDate.Date).Days;
        var passed = age <= MaxAgeDays;

        return Task.FromResult(new ValidationRuleResult
        {
            RuleName = RuleName,
            Passed = passed,
            FailureReason = passed
                ? null
                : $"Invoice is {age} days old, exceeding the maximum of {MaxAgeDays} days for reclaim eligibility."
        });
    }
}
