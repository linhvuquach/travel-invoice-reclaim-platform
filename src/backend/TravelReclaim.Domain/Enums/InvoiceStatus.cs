namespace TravelReclaim.Domain;

public enum InvoiceStatus
{
    Draft,
    Submitted,
    Validated,
    Rejected,
    ReclaimPending,
    Reclaimed
}
