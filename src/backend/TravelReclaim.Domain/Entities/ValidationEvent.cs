namespace TravelReclaim.Domain.Entities;

public class ValidationEvent
{
    public string? Id { get; set; }
    public Guid InvoiceId { get; set; }
    public string HotleName { get; set; } = string.Empty;
    public Guid? TenantId { get; set; }
    public DateTime Timestamp { get; set; }
    public List<ValidationRuleResult> Results { get; set; } = [];
    public string OverallOutcome { get; set; } = string.Empty;
    public long TotalExecutionTimeMs { get; set; }
    public string TriggedBy { get; set; } = "system";
}

public class ValidationRuleResult
{
    public string RuleName { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string? FailureReason { get; set; }
    public long ExecutionTimeMs { get; set; }
}