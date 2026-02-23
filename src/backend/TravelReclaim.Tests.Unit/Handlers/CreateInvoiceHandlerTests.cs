using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Application.Invoices.Commands;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Tests.Unit.Handlers;

public class CreateInvoiceHandlerTests
{
    // Mocking
    private readonly Mock<IInvoiceRepository> _invoiceRepoMock = new();
    private readonly Mock<IAuditService> _auditServiceMock = new();

    private CreateInvoiceHandler CreateHandler() => new(_invoiceRepoMock.Object, _auditServiceMock.Object);

    [Fact]
    public async Task HandleAsync_ValidCommand_ReturnsInvoiceResponse()
    {
        // Given
        _invoiceRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Invoice inv, CancellationToken _) => inv);

        var command = new CreateInvoiceCommand(
            "Test Hotel", "INV-001", DateTime.UtcNow.AddDays(-5),
            1000m, 190m, "EUR", "Test");

        // When
        var handler = CreateHandler();
        var result = await handler.HandleAsync(command);

        // Then
        Assert.NotNull(result);
        Assert.Equal("Test Hotel", result.HotelName);
        Assert.Equal("INV-001", result.InvoiceNumber);
        Assert.Equal(1000m, result.TotalAmount);
        Assert.Equal("Submitted", result.Status);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_LogsAuditEvent()
    {
        // Given
        _invoiceRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Invoice inv, CancellationToken _) => inv);

        var command = new CreateInvoiceCommand(
            "Test Hotel", "INV-001", DateTime.UtcNow.AddDays(-5),
            1000m, 190m, "EUR", null);

        // When
        var handler = CreateHandler();
        await handler.HandleAsync(command);

        // Then
        _auditServiceMock.Verify(
            a => a.LogEventAsync(
                It.Is<AuditEvent>(e => e.EntityType == "Invoice" && e.Action == "Created"),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}