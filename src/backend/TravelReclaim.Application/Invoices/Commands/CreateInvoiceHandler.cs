using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Invoices.Commands;

public class CreateInvoiceHandler(IInvoiceRepository invoiceRepository) : ICommandHandler<CreateInvoiceCommand, InvoiceResponse>
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

        return invoice.ToResponse();
    }
}