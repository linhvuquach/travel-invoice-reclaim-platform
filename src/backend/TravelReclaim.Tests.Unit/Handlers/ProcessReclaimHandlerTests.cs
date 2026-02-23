using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TravelReclaim.Application.Exceptions;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Application.Reclaims.Commands;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;
using TravelReclaim.Tests.Unit.Helpers;

namespace TravelReclaim.Tests.Unit.Handlers;

public class ProcessReclaimHandlerTests
{
    // Mocking
    private readonly Mock<IInvoiceRepository> _invoiceRepoMock = new();
    private readonly Mock<IReclaimRepository> _reclaimRepoMock = new();
    private readonly Mock<IAuditService> _auditServiceMock = new();

    private ProcessReclaimHandler CreateHandler(params IValidationRule[] rules)
        => new(_invoiceRepoMock.Object, _reclaimRepoMock.Object, rules, _auditServiceMock.Object);

    [Fact]
    public async Task HandleAsync_AllRulePass_CreatesReclaimWithPendingStatus()
    {
        // Given
        var invoice = TestInvoiceFactory.CreateValid();
        var invoiceId = invoice.Id;
        _invoiceRepoMock.Setup(r => r.GetByIdAsync(invoiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);
        _reclaimRepoMock.Setup(r => r.AddAsync(It.IsAny<Reclaim>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reclaim r, CancellationToken _) => r);
        _reclaimRepoMock.Setup(r => r.GetByIdWithInvoiceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Reclaim.Create(invoiceId, 190m));

        // When
        var passingRule = CreatePassingRule();
        var handler = CreateHandler(passingRule.Object);

        var result = await handler.HandleAsync(new ProcessReclaimCommand(invoiceId));

        // Then
        Assert.True(result.IsValid);
        Assert.NotNull(result.Reclaim);
        _reclaimRepoMock.Verify(r => r.AddAsync(It.IsAny<Reclaim>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_AllRuleFails_RejectsInvoice()
    {
        // Given
        var invoice = TestInvoiceFactory.CreateValid();
        _invoiceRepoMock.Setup(r => r.GetByIdAsync(invoice.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        // When
        var passingRule = CreatePassingRule();
        var failingRule = CreateFailingRule("Amount too high");
        var handler = CreateHandler(passingRule.Object, failingRule.Object);

        var result = await handler.HandleAsync(new ProcessReclaimCommand(invoice.Id));

        // Then
        Assert.False(result.IsValid);
        Assert.Null(result.Reclaim);
        Assert.Contains(result.ValidationResults, r => !r.Passed);
        _invoiceRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()), Times.Once);

    }

    [Fact]
    public async Task HandleAsync_InvoiceNotFound_ThrowsEntityNotFoundException()
    {
        // Given
        _invoiceRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Invoice?)null);

        // When
        var handler = CreateHandler();

        // Then
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => handler.HandleAsync(new ProcessReclaimCommand(Guid.NewGuid()))
        );
    }

    [Fact]
    public async Task HandleAsync_InvoiceNotSubmitted_ThrowsBusinessRuleException()
    {
        var invoice = TestInvoiceFactory.CreateValid();
        // Invoice is in Submitted status by default, let's reject it to change status
        invoice.MarkAsRejected("test");

        // When
        _invoiceRepoMock.Setup(r => r.GetByIdAsync(invoice.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        var handler = CreateHandler();

        // Then
        await Assert.ThrowsAsync<BusinessRuleException>(
            () => handler.HandleAsync(new ProcessReclaimCommand(invoice.Id))
        );
    }

    [Fact]
    public async Task HandleAsync_AllRulePass_LogsValidationEvent()
    {
        // Given
        var invoice = TestInvoiceFactory.CreateValid();
        var invoiceId = invoice.Id;
        _invoiceRepoMock.Setup(r => r.GetByIdAsync(invoiceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);
        _reclaimRepoMock.Setup(r => r.AddAsync(It.IsAny<Reclaim>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reclaim r, CancellationToken _) => r);
        _reclaimRepoMock.Setup(r => r.GetByIdWithInvoiceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Reclaim.Create(invoiceId, 190m));

        // When
        var passingRule = CreatePassingRule();
        var handler = CreateHandler(passingRule.Object);

        var result = await handler.HandleAsync(new ProcessReclaimCommand(invoiceId));

        // Then
        _auditServiceMock.Verify(
            a => a.LogValidationAsync(
                It.Is<ValidationEvent>(e => e.OverallOutcome == "Passed"),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    private static Mock<IValidationRule> CreatePassingRule()
    {
        var mock = new Mock<IValidationRule>();
        mock.Setup(r => r.RuleName).Returns("TestPassingRule");
        mock.Setup(r => r.ValidateAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationRuleResult { RuleName = "TestPassingRule", Passed = true });

        return mock;
    }

    private static Mock<IValidationRule> CreateFailingRule(string reason)
    {
        var mock = new Mock<IValidationRule>();
        mock.Setup(r => r.RuleName).Returns("TestFailingRule");
        mock.Setup(r => r.ValidateAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationRuleResult { RuleName = "TestPassingRule", Passed = false, FailureReason = reason });

        return mock;
    }
}