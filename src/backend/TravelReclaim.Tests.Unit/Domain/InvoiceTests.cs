using TravelReclaim.Domain;

namespace TravelReclaim.Tests.Unit.Domain;

public class InvoiceTests
{
    [Fact]
    public void Create_ValidParameters_ReturnInvoiceWithSubmittedStatus()
    {
        var invoice = Invoice.Create("Hotel", "INV-001", DateTime.UtcNow, 1000m, 190m, "EUR");

        Assert.Equal(InvoiceStatus.Submitted, invoice.Status);
        Assert.Equal("Hotel", invoice.HotelName);
        Assert.Equal(1000m, invoice.TotalAmount);
        Assert.NotEqual(Guid.Empty, invoice.Id);
    }

    [Fact]
    public void MarkAsValidated_FromSubmitted_Succeeds()
    {
        var invoice = Invoice.Create("Hotel", "INV-001", DateTime.UtcNow, 1000m, 190m, "EUR");

        invoice.MarkAsValidated();

        Assert.Equal(InvoiceStatus.Validated, invoice.Status);
    }

    [Fact]
    public void MarkAsValidated_FromRejected_ThrowsInvalidOperation()
    {
        var invoice = Invoice.Create("Hotel", "INV-001", DateTime.UtcNow, 1000m, 190m, "EUR");

        invoice.MarkAsRejected("Reason");

        Assert.Throws<InvalidOperationException>(() => invoice.MarkAsValidated());
    }

    [Fact]
    public void MarkAsReclaimPending_FromSubmitted_Succeeds()
    {
        var invoice = Invoice.Create("Hotel", "INV-001", DateTime.UtcNow, 1000m, 190m, "EUR");

        invoice.MarkAsReclaimPending();

        Assert.Equal(InvoiceStatus.ReclaimPending, invoice.Status);
    }

    [Fact]
    public void MarkAsReclaimed_FromReclaimPending_Succeeds()
    {
        var invoice = Invoice.Create("Hotel", "INV-001", DateTime.UtcNow, 1000m, 190m, "EUR");
        invoice.MarkAsReclaimPending();

        invoice.MarkAsReclaimed();

        Assert.Equal(InvoiceStatus.Reclaimed, invoice.Status);
    }

    [Fact]
    public void MarkAsReclaimed_FromSubmitted_ThrowsInvalidOperation()
    {
        var invoice = Invoice.Create("Hotel", "INV-001", DateTime.UtcNow, 1000m, 190m, "EUR");

        Assert.Throws<InvalidOperationException>(() => invoice.MarkAsReclaimed());
    }
}