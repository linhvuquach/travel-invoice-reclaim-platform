using MongoDB.Driver;
using TravelReclaim.Domain.Entities;

namespace TravelReclaim.Infrastructure.Persistence.MongoDB;

public static class MongoDbIndexSetup
{
    public static async Task EnsureIndexesAsync(MongoDbContext context)
    {
        await CreateAuditEventIndexesAsync(context.AuditEvents);
        await CreateValidationEventIndexesAsync(context.ValidationEvents);
    }

    private static async Task CreateAuditEventIndexesAsync(IMongoCollection<AuditEvent> collection)
    {
        var indexes = new List<CreateIndexModel<AuditEvent>>
        {
            // Query by entity + chronological order
            new(
                Builders<AuditEvent>.IndexKeys
                    .Ascending(e => e.EntityId)
                    .Descending(e => e.Timestamp),
                new CreateIndexOptions {Name = "IX_EntityId_Timestamp"}
            ),
            // Query by tenant + chronological order
            new (
                Builders<AuditEvent>.IndexKeys
                    .Ascending(e => e.TenantId)
                    .Descending(e => e.Timestamp),
                new CreateIndexOptions {Name = "IX_TenantId_Timestamp"}
            ),
            // TTL: auto-expire after 365 days
            new(
                Builders<AuditEvent>.IndexKeys.Ascending(e => e.Timestamp),
                new CreateIndexOptions
                {
                    Name = "IX_Timestamp_TTL",
                    ExpireAfter = TimeSpan.FromDays(365)
                }
            ),
            // Filter by action and entity type
            new (
                Builders<AuditEvent>.IndexKeys
                    .Ascending(e => e.Action)
                    .Ascending(e => e.EntityType),
                new CreateIndexOptions {Name = "IX_Action_EntityType"}
            )
        };

        await collection.Indexes.CreateManyAsync(indexes);
    }

    private static async Task CreateValidationEventIndexesAsync(IMongoCollection<ValidationEvent> collection)
    {
        var indexes = new List<CreateIndexModel<ValidationEvent>>
        {
            // Query by invoice + chronological order
            new(
                Builders<ValidationEvent>.IndexKeys
                    .Ascending(e => e.InvoiceId)
                    .Descending(e => e.Timestamp),
                new CreateIndexOptions { Name = "IX_InvoiceId_Timestamp" }
            ),
            // Filter by outcome + chronological order
            new(
                Builders<ValidationEvent>.IndexKeys
                    .Ascending(e => e.OverallOutcome)
                    .Descending(e => e.Timestamp),
                new CreateIndexOptions { Name = "IX_OverallOutcome_Timestamp" }
            ),
            // Query by tenant + chronological order
            new(
                Builders<ValidationEvent>.IndexKeys
                    .Ascending(e => e.TenantId)
                    .Descending(e => e.Timestamp),
                new CreateIndexOptions { Name = "IX_TenantId_Timestamp" }
            )
        };

        await collection.Indexes.CreateManyAsync(indexes);
    }
}