using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelReclaim.Infrastructure.Persistence.MongoDB;

namespace TravelReclaim.Infrastructure.Extensions
{
    public static class PersistenceExtension
    {
        public static void AddPersistenceService(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // SQL server
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            // MongoDB
            serviceCollection.AddSingleton<MongoDbContext>();
        }
    }
}