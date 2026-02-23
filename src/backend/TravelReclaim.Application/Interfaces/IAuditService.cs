using TravelReclaim.Domain.Entities;

namespace TravelReclaim.Application.Interfaces;

public interface IAuditService
{
    Task LogEventAsync(AuditEvent auditEvent, CancellationToken ct = default);
    Task LogValidationAsync(ValidationEvent validationEvent, CancellationToken ct = default);
    Task<IReadOnlyList<AuditEvent>> GetAuditTrailAsync(Guid entityId, CancellationToken ct = default);
    Task<ValidationOutcomeReport> GetValidationOutcomeReportAsync(AuditReportFilter filter, CancellationToken ct = default);
    Task<IReadOnlyList<ValidationByHotelEntry>> GetValidationByHoltelAsync(AuditReportFilter filter, CancellationToken ct = default);
    Task<IReadOnlyList<ValidationByRuleEntry>> GetValidationByRuleAsync(AuditReportFilter filter, CancellationToken ct = default);
}

public record ValidationByRuleEntry(string RuleName, int FailureCount, double AvgExecutionTimeMs);

public record ValidationByHotelEntry(string HotleName, int TotalValidations, int Passed, int Failed);

public record AuditReportFilter(DateTime? FromDate = null, DateTime? ToDate = null);

public record ValidationOutcomeReport(
    int TotalValidations,
    int Passed,
    int Failed,
    decimal PassRate
);