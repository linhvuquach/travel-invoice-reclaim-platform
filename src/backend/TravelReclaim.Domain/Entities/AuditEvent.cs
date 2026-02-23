namespace TravelReclaim.Domain.Entities;

public class AuditEvent
{
    public string? Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string PerformedBy { get; set; } = string.Empty;
    public Guid? TenantId { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}
