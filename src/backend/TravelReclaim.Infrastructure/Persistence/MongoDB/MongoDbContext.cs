using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using TravelReclaim.Domain.Entities;
using GuidRepresentation = MongoDB.Bson.GuidRepresentation;

namespace TravelReclaim.Infrastructure.Persistence.MongoDB;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB")
            ?? throw new InvalidOperationException("MongoDB connection string is not configured");

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("TravelReclaimAudit");

        RegisterClassMaps();
    }

    public IMongoCollection<AuditEvent> AuditEvents
        => _database.GetCollection<AuditEvent>("auditEvents");

    public IMongoCollection<ValidationEvent> ValidationEvents
        => _database.GetCollection<ValidationEvent>("validationEvents");

    private static void RegisterClassMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(AuditEvent)))
        {
            BsonClassMap.RegisterClassMap<AuditEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.MapMember(c => c.EntityId)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapMember(c => c.TenantId)
                    .SetSerializer(new NullableSerializer<Guid>(new GuidSerializer(GuidRepresentation.Standard)));
                cm.SetIgnoreExtraElements(true);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(ValidationEvent)))
        {
            BsonClassMap.RegisterClassMap<ValidationEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
                cm.MapMember(c => c.InvoiceId)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapMember(c => c.TenantId)
                    .SetSerializer(new NullableSerializer<Guid>(new GuidSerializer(GuidRepresentation.Standard)));
                cm.SetIgnoreExtraElements(true);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(ValidationRuleResult)))
        {
            BsonClassMap.RegisterClassMap<ValidationRuleResult>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
