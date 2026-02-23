using Moq;
using TravelReclaim.Application.Validators;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;
using TravelReclaim.Tests.Unit.Helpers;

namespace TravelReclaim.Tests.Unit.Validators;

public class DuplicateInvoiceRuleTests
{
    // Mocking
    private readonly Mock<IReclaimRepository> _reclaimRepoMock = new();

    [Fact]
    public async Task ValidateAsync_NoExistReclaim_Passes()
    {
        // Given
        _reclaimRepoMock
            .Setup(r => r.GetByInvoiceIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reclaim?)null);
        var rule = new DuplicateInvoiceRule(_reclaimRepoMock.Object);
        var invoice = TestInvoiceFactory.CreateValid();

        // When
        var result = await rule.ValidateAsync(invoice);

        // Then
        Assert.True(result.Passed);
        Assert.Null(result.FailureReason);
    }

    [Fact]
    public async Task ValidateAsync_ExistReclaim_Fails()
    {
        // Given
        var existingReclaim = Reclaim.Create(Guid.NewGuid(), 300m);

        _reclaimRepoMock
            .Setup(r => r.GetByInvoiceIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingReclaim);

        var rule = new DuplicateInvoiceRule(_reclaimRepoMock.Object);
        var invoice = TestInvoiceFactory.CreateValid();

        // When
        var result = await rule.ValidateAsync(invoice);

        // Then
        Assert.False(result.Passed);
        Assert.NotNull(result.FailureReason);
    }
}