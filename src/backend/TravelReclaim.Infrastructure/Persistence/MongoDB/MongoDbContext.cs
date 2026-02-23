using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Entities;

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
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.SetIgnoreExtraElements(true);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(ValidationEvent)))
        {
            BsonClassMap.RegisterClassMap<ValidationEvent>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
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
