using TravelReclaim.Domain.Enums;

namespace TravelReclaim.Domain.Entities;

public class Reclaim
{
    public Guid Id { get; private set; }
    public Guid InvoiceId { get; private set; }
    public decimal EligibleAmount { get; private set; }
    public ReclaimStatus Status { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime? ProcessedDate { get; private set; }
    public string? RejectionReason { get; private set; }
    public string? ProcessedBy { get; private set; }

    // Navigation
    public Invoice Invoice { get; private set; } = null!;

    // EF Core requires a parameterless constructor
    private Reclaim() { }

    public static Reclaim Create(Guid invoiceId, decimal eligibleAmount)
    {
        return new Reclaim
        {
            Id = Guid.NewGuid(),
            InvoiceId = invoiceId,
            EligibleAmount = eligibleAmount,
            Status = ReclaimStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };
    }

    public void Approve(string processedBy)
    {
        CheckValidPendingStatus();

        Status = ReclaimStatus.Approved;
        ProcessedDate = DateTime.UtcNow;
        ProcessedBy = processedBy;
    }

    public void Reject(string reason, string processedBy)
    {
        CheckValidPendingStatus();

        Status = ReclaimStatus.Rejected;
        RejectionReason = reason;
        ProcessedDate = DateTime.UtcNow;
        ProcessedBy = processedBy;
    }

    private void CheckValidPendingStatus()
    {
        if (Status != ReclaimStatus.Pending)
            throw new InvalidOperationException(
                $"Cannot approve reclaim in '{Status}'. Must be '{ReclaimStatus.Pending}'"
            );
    }
}