using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Interfaces;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Application.Invoices.Commands;

public class CreateInvoiceHandler(IInvoiceRepository invoiceRepository,
    IEventBus eventBus) : ICommandHandler<CreateInvoiceCommand, InvoiceResponse>
{
    public async Task<InvoiceResponse> HandleAsync(CreateInvoiceCommand command, CancellationToken ct = default)
    {
        var invoice = Invoice.Create(
            hotelName: command.HotelName,
            invoiceNumber: command.InvoiceNumber,
            issueDate: command.IssueDate,
            totalAmount: command.TotalAmount,
            vatAmount: command.VatAmount,
            currency: command.Currency,
            description: command.Description);

        await invoiceRepository.AddAsync(invoice, ct);

        await eventBus.PublishAsync(new InvoiceCreatedEvent(
                invoice.Id,
                invoice.HotelName,
                "system", // JWT user later in Phase 3,
                DateTime.UtcNow
            ), ct);

        return invoice.ToResponse();
    }
}