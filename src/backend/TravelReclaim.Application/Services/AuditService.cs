using MongoDB.Driver;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Infrastructure.Persistence.MongoDB;

namespace TravelReclaim.Application.Services;

public class AuditService(MongoDbContext context) : IAuditService
{
    public async Task<IReadOnlyList<AuditEvent>> GetAuditTrailAsync(Guid entityId, CancellationToken ct = default)
    {
        return await context.AuditEvents
            .Find(e => e.EntityId == entityId)
            .SortByDescending(e => e.Timestamp)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ValidationByHotelEntry>> GetValidationByHoltelAsync(AuditReportFilter filter, CancellationToken ct = default)
    {
        var matchFilter = BuildDateFilter(filter);

        var pipeline = context.ValidationEvents.Aggregate()
            .Match(matchFilter)
            .Group(
                e => e.HotleName,
                g => new ValidationByHotelEntry(
                    g.Key,
                    g.Count(),
                    g.Sum(e => e.OverallOutcome == "Passed" ? 1 : 0),
                    g.Sum(e => e.OverallOutcome == "Failed" ? 1 : 0)
                ))
            .SortByDescending(r => r.TotalValidations);

        return await pipeline.ToListAsync();
    }

    public async Task<IReadOnlyList<ValidationByRuleEntry>> GetValidationByRuleAsync(AuditReportFilter filter, CancellationToken ct = default)
    {
        var matchFilter = BuildDateFilter(filter);

        var pipeline = context.ValidationEvents.Aggregate()
            .Match(matchFilter)
            .Match(e => e.OverallOutcome == "Failed")
            .Unwind<ValidationEvent, UnwoundValidationEvent>(e => e.Results)
            .Match(u => u.Results.Passed == false)
            .Group(
                u => u.Results.RuleName,
                g => new ValidationByRuleEntry(
                    g.Key,
                    g.Count(),
                    g.Average(u => u.Results.ExecutionTimeMs)
                ))
            .SortByDescending(r => r.FailureCount);

        return await pipeline.ToListAsync();
    }

    public async Task<ValidationOutcomeReport> GetValidationOutcomeReportAsync(AuditReportFilter filter, CancellationToken ct = default)
    {
        var matchFilter = BuildDateFilter(filter);

        var pipeline = context.ValidationEvents.Aggregate()
            .Match(matchFilter)
            .Group(
                e => 1,
                g => new
                {
                    TotalValidations = g.Count(),
                    Passed = g.Sum(e => e.OverallOutcome == "Passed" ? 1 : 0),
                    Failed = g.Sum(e => e.OverallOutcome == "Failed" ? 1 : 0)
                }
            );

        var result = await pipeline.FirstOrDefaultAsync(ct);

        if (result is null)
            return new ValidationOutcomeReport(0, 0, 0, 0);

        var passRate = result.TotalValidations > 0
            ? Math.Round((decimal)result.Passed / result.TotalValidations * 100, 2)
            : 0;

        return new ValidationOutcomeReport(result.TotalValidations, result.Passed, result.Failed, passRate);
    }

    public async Task LogEventAsync(AuditEvent auditEvent, CancellationToken ct = default)
    {
        auditEvent.Timestamp = DateTime.UtcNow;
        await context.AuditEvents.InsertOneAsync(auditEvent, cancellationToken: ct);
    }

    public async Task LogValidationAsync(ValidationEvent validationEvent, CancellationToken ct = default)
    {
        validationEvent.Timestamp = DateTime.UtcNow;
        await context.ValidationEvents.InsertOneAsync(validationEvent, cancellationToken: ct);
    }

    private static FilterDefinition<ValidationEvent> BuildDateFilter(AuditReportFilter filter)
    {
        var builder = Builders<ValidationEvent>.Filter;
        var filters = new List<FilterDefinition<ValidationEvent>>();

        if (filter.FromDate.HasValue)
            filters.Add(builder.Gte(e => e.Timestamp, filter.FromDate.Value));

        if (filter.ToDate.HasValue)
            filters.Add(builder.Gte(e => e.Timestamp, filter.ToDate.Value));

        return filters.Count > 0 ? builder.And(filters) : builder.Empty;
    }

    // Helper calss for Unwind operation
    private class UnwoundValidationEvent
    {
        public Guid InvoiceId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public ValidationRuleResult Results { get; set; } = null!;
        public string OverallOutcome { get; set; } = string.Empty;
    }
}