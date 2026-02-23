using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Invoices.Commands;

public class CreateInvoiceHandler(IInvoiceRepository invoiceRepository,
    IAuditService auditService) : ICommandHandler<CreateInvoiceCommand, InvoiceResponse>
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

        await auditService.LogEventAsync(new AuditEvent
        {
            EntityId = invoice.Id,
            EntityType = "Invoice",
            Action = "Created",
            PerformedBy = "system" // JWT user later in Phase 3
        }, ct);

        return invoice.ToResponse();
    }
}