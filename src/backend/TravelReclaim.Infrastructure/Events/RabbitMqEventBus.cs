using MassTransit;
using TravelReclaim.Infrastructure.Events.Abstractions;

namespace TravelReclaim.Infrastructure.Events;

/// <summary>
/// Publishes domain events to RabbitMQ via MassTransit.
/// The Application layer only knows <see cref="IEventBus"/> — no MassTransit types leak upward.
/// </summary>
public class RabbitMqEventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default) where TEvent : class =>
        publishEndpoint.Publish(@event, ct);
}