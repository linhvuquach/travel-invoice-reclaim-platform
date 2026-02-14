using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;

namespace TravelReclaim.Application.Invoices.Commands;

public record CreateInvoiceCommand(
    string HotelName,
    string InvoiceNumber,
    DateTime IssueDate,
    decimal TotalAmount,
    decimal VatAmount,
    string Currency,
    string? Description
) : ICommand<InvoiceResponse>;