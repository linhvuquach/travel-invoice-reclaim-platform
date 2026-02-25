namespace TravelReclaim.Infrastructure.Events.Models;

public record ReclaimApprovedEvent(
    Guid ReclaimId,
    string ApprovedBy,
    DateTime Timestamp);
