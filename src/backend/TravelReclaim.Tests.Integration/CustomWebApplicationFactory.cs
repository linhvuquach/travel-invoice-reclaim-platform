using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using TravelReclaim.Application.Interfaces;
using TravelReclaim.Infrastructure;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Persistence.MongoDB;

namespace TravelReclaim.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"TestDB_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove All Dbcontext-related registrations to avoid dual-provider conflict
            var dbContextDescriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                            || d.ServiceType == typeof(DbContextOptions)
                            || d.ServiceType.FullName?.Contains("EntityFrameworkCore") == true)
                .ToList();

            foreach (var descriptor in dbContextDescriptors)
            {
                services.Remove(descriptor);
            }

            // Re-add with Inmemory provider only
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(_dbName));

            // Remove MongoDbContext to avoid real MongoDB connection
            RemoveService<MongoDbContext>(services);

            // Replace IAuditservice with a no-op mock
            RemoveService<IAuditService>(services);
            services.AddScoped<IAuditService>(_ => new Mock<IAuditService>().Object);

            // Remove MassTransit hosted services to prevent RabbitMQ connection attempts
            var massTransitHostedServices = services
                .Where(d => d.ServiceType == typeof(IHostedService)
                    && d.ImplementationType?.Assembly.GetName().Name?.StartsWith("MassTransit") == true)
                .ToList();

            foreach (var descriptor in massTransitHostedServices)
                services.Remove(descriptor);

            // Replace IEventBus with a no-op mock so handlers can publish without a broker
            RemoveService<IEventBus>(services);
            services.AddScoped<IEventBus>(_ => new Mock<IEventBus>().Object);
        });
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor != null)
            services.Remove(descriptor);
    }
}
