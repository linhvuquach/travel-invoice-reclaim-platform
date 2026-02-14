using TravelReclaim.Domain;

namespace TravelReclaim.Application.DTOs;

public static class InvoiceMapper
{
    public static InvoiceResponse ToResponse(this Invoice invoice)
    {
        return new InvoiceResponse(
            Id: invoice.Id,
            HotelName: invoice.HotelName,
            InvoiceNumber: invoice.InvoiceNumber,
            IssueDate: invoice.IssueDate,
            SubmissionDate: invoice.SubmissionDate,
            TotalAmount: invoice.TotalAmount,
            VatAmount: invoice.VatAmount,
            Currency: invoice.Currency,
            Status: invoice.Status.ToString(),
            Description: invoice.Description,
            Payment: invoice.Payment is not null
                ? new PaymentResponse(
                    Id: invoice.Payment.Id,
                    Amount: invoice.Payment.Amount,
                    PaymentDate: invoice.Payment.PaymentDate,
                    Method: invoice.Payment.Method.ToString(),
                    Status: invoice.Payment.Status.ToString())
                : null
        );
    }
}