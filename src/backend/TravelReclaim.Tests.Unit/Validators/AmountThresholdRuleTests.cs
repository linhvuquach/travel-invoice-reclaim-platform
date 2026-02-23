using TravelReclaim.Application.Validators;
using TravelReclaim.Tests.Unit.Helpers;

namespace TravelReclaim.Tests.Unit.Validators;

public class AmountThresholdRuleTests
{
    private readonly AmountThresholdRule _rule = new();

    [Theory]
    [InlineData(100)]
    [InlineData(999_999.99)]
    [InlineData(1_000_000)]
    public async Task ValidateAsync_ValidAmount_Passes(decimal amount)
    {
        var invoice = TestInvoiceFactory.CreateValid(totalAmount: amount);

        var result = await _rule.ValidateAsync(invoice);

        Assert.True(result.Passed);
        Assert.Null(result.FailureReason);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    [InlineData(1_000_001)]
    public async Task ValidateAsync_InvalidAmount_Failes(decimal amount)
    {
        // Given
        var invoice = TestInvoiceFactory.CreateValid(totalAmount: amount);

        // When
        var result = await _rule.ValidateAsync(invoice);

        // Then
        Assert.False(result.Passed);
        Assert.NotNull(result.FailureReason);
    }

    [Fact]
    public void RuleName_ReturnsCorrectName()
    {
        Assert.Equal("AmountThresholdRule", _rule.RuleName);
    }
}