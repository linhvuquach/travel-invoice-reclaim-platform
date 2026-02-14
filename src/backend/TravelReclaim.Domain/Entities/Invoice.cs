namespace TravelReclaim.Domain;

public class Invoice
{
    public Guid Id { get; private set; }
    public string HotelName { get; private set; } = string.Empty;
    public string InvoiceNumber { get; private set; } = string.Empty;
    public DateTime IssueDate { get; private set; }
    public DateTime SubmissionDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal VatAmount { get; private set; }
    public string Currency { get; private set; } = "EUR";
    public InvoiceStatus Status { get; private set; }
    public string? Description { get; private set; }

    // Navigation
    public Payment? Payment { get; private set; }

    // EF Core requires a parameterless constructor
    private Invoice() { }

    public static Invoice Create(
        string hotelName,
        string invoiceNumber,
        DateTime issueDate,
        decimal totalAmount,
        decimal vatAmount,
        string currency,
        string? description = null)
    {
        return new Invoice
        {
            Id = Guid.NewGuid(),
            HotelName = hotelName,
            InvoiceNumber = invoiceNumber,
            IssueDate = issueDate,
            SubmissionDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            VatAmount = vatAmount,
            Currency = currency,
            Status = InvoiceStatus.Submitted,
            Description = description
        };
    }

    public void MarkAsValidated()
    {
        CheckValidStatus(InvoiceStatus.Submitted);

        Status = InvoiceStatus.Validated;
    }

    public void MarkAsRejected()
    {
        CheckValidStatus(InvoiceStatus.Submitted);

        Status = InvoiceStatus.Rejected;
    }

    public void MarkAsReclaimPending()
    {
        CheckValidStatus(InvoiceStatus.Submitted);

        Status = InvoiceStatus.ReclaimPending;
    }

    public void MarkAsReclaimed()
    {
        CheckValidStatus(InvoiceStatus.ReclaimPending);

        Status = InvoiceStatus.Reclaimed;
    }

    private void CheckValidStatus(InvoiceStatus expectedStatus)
    {
        if (Status != expectedStatus)
            throw new InvalidOperationException(
                $"Canot validate invoice in '{Status}'. Must be '{expectedStatus}'"
            );
    }
}
