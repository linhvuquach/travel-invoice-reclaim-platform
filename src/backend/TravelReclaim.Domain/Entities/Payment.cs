namespace TravelReclaim.Domain;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid InvoiceId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }

    // Navigation
    public Invoice Invoice { get; private set; } = null!;

    // EF Core requires a parameterless constructor
    private Payment() { }

    public static Payment Create(
        Guid invoiceId,
        decimal amount,
        PaymentMethod method)
    {
        return new Payment
        {
            Id = Guid.NewGuid(),
            InvoiceId = invoiceId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            Method = method,
            Status = PaymentStatus.Pending
        };
    }

    public void MarkAsCompleted()
    {
        CheckPaymentPendingStatus();

        Status = PaymentStatus.Completed;
    }

    public void MakrAsFailed()
    {
        CheckPaymentPendingStatus();

        Status = PaymentStatus.Failed;
    }

    private void CheckPaymentPendingStatus()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException(
                $"Cannot complete payment in '{Status}' status. Must be '{PaymentStatus.Pending}'.");
    }
}
