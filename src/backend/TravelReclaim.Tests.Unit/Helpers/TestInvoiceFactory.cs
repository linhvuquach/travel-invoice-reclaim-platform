using TravelReclaim.Domain;

namespace TravelReclaim.Tests.Unit.Helpers;

public static class TestInvoiceFactory
{
    public static Invoice CreateValid(
        decimal totalAmount = 1000m,
        decimal vatAmount = 190m,
        DateTime? issueDate = null,
        string invoiceNumber = "INV-001",
        string hotelName = "Test Hotel")
    {
        return Invoice.Create(
            hotelName: hotelName,
            invoiceNumber: invoiceNumber,
            issueDate: issueDate ?? DateTime.UtcNow.AddDays(-10),
            totalAmount: totalAmount,
            vatAmount: vatAmount,
            currency: "EUR",
            description: "Test invoice");
    }
}