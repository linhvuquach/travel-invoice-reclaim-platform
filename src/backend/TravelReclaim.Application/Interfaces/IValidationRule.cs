using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;

namespace TravelReclaim.Application.Interfaces;

public interface IValidationRule
{
    string RuleName { get; }
    Task<ValidationRuleResult> ValidateAsync(Invoice invoice, CancellationToken ct = default);
}