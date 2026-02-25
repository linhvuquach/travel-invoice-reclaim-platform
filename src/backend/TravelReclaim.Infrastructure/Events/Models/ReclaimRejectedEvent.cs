namespace TravelReclaim.Infrastructure.Events.Models;

public record ReclaimRejectedEvent(
    Guid ReclaimId,
    string Reason,
    string RejectedBy,
    DateTime Timestamp);
