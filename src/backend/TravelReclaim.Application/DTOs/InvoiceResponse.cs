namespace TravelReclaim.Application.DTOs;

public record InvoiceResponse
(
    Guid Id,
    string HotelName,
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime SubmissionDate,
    decimal TotalAmount,
    decimal VatAmount,
    string Currency,
    string Status,
    string? Description,
    PaymentResponse? Payment
);

public record PaymentResponse(
    Guid Id,
    decimal Amount,
    DateTime PaymentDate,
    string Method,
    string Status
);