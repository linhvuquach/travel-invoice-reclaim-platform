namespace TravelReclaim.Infrastructure.Events.Models;

public record InvoiceCreatedEvent(
    Guid InvoiceId,
    string HotelName,
    string PerformedBy,
    DateTime Timestamp);
