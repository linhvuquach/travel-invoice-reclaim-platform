using TravelReclaim.Application.Validators;
using TravelReclaim.Tests.Unit.Helpers;

namespace TravelReclaim.Tests.Unit.Validators;

public class VatRatioRuleTests
{
    private readonly VatRatioRule _rule = new();

    [Theory]
    [InlineData(1000, 250)]
    [InlineData(1000, 0)]
    [InlineData(1000, 100)]
    [InlineData(1000, 190)]
    public async Task ValidateAsync_ValidVatRatio_Passes(decimal total, decimal vat)
    {
        var invoice = TestInvoiceFactory.CreateValid(totalAmount: total, vatAmount: vat);

        var result = await _rule.ValidateAsync(invoice);

        Assert.True(result.Passed);
        Assert.Null(result.FailureReason);
    }

    [Theory]
    [InlineData(100, 100)]
    [InlineData(1000, 260)]
    [InlineData(1000, 500)]
    public async Task ValidateAsync_ExcessVatRatio_Fails(decimal total, decimal vat)
    {
        var invoice = TestInvoiceFactory.CreateValid(totalAmount: total, vatAmount: vat);

        var result = await _rule.ValidateAsync(invoice);

        Assert.False(result.Passed);
        Assert.NotNull(result.FailureReason);
    }

    public async Task ValidateAsync_ZeroTotalAmount_Fails()
    {
        var invoice = TestInvoiceFactory.CreateValid(totalAmount: 0);

        var result = await _rule.ValidateAsync(invoice);

        Assert.False(result.Passed);
        Assert.Contains("zero", result.FailureReason);
    }
}