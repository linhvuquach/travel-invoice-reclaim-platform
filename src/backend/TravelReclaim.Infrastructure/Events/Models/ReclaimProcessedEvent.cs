namespace TravelReclaim.Infrastructure.Events.Models;

/// <summary>
/// Published after reclaim validation runs.
/// <see cref="Outcome"/> is "Created" when all rules pass, "Rejected" when any rule fails.
/// <see cref="ReclaimId"/> is null on rejection.
/// </summary>
public record ReclaimProcessedEvent(
    Guid? ReclaimId,
    Guid InvoiceId,
    string Outcome,
    string? RejectionReason,
    string PerformedBy,
    DateTime Timestamp);
