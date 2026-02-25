using Moq;
using TravelReclaim.Application.Invoices.Commands;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Interfaces;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Tests.Unit.Handlers;

public class CreateInvoiceHandlerTests
{
    // Mocking
    private readonly Mock<IInvoiceRepository> _invoiceRepoMock = new();
    private readonly Mock<IEventBus> _eventBusMock = new();

    private CreateInvoiceHandler CreateHandler() => new(_invoiceRepoMock.Object, _eventBusMock.Object);

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
    public async Task HandleAsync_ValidCommand_PublishesInvoiceCreatedEvent()
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
        _eventBusMock.Verify(
            a => a.PublishAsync(
                It.Is<InvoiceCreatedEvent>(e => e.HotelName == "Test Hotel"),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}