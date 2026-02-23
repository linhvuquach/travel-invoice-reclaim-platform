using TravelReclaim.Application.Validators;
using TravelReclaim.Tests.Unit.Helpers;

namespace TravelReclaim.Tests.Unit.Validators;

public class InvoiceAgeRuleTests
{
    private readonly InvoiceAgeRule _rule = new();

    [Theory]
    [InlineData(0)]
    [InlineData(30)]
    [InlineData(89)]
    [InlineData(90)]
    public async Task ValidateAsync_WithinMaxAge_Passes(int dayAgo)
    {
        var invoice = TestInvoiceFactory.CreateValid(issueDate: DateTime.UtcNow.AddDays(-dayAgo));

        var result = await _rule.ValidateAsync(invoice);

        Assert.True(result.Passed);
        Assert.Null(result.FailureReason);
    }

    [Theory]
    [InlineData(91)]
    [InlineData(180)]
    [InlineData(365)]
    public async Task ValidateAsync_ExceedMaxAge_Fails(int dayAgo)
    {
        var invoice = TestInvoiceFactory.CreateValid(issueDate: DateTime.UtcNow.AddDays(-dayAgo));

        var result = await _rule.ValidateAsync(invoice);

        Assert.False(result.Passed);
        Assert.Contains("exceeding the maximum", result.FailureReason);
    }
}