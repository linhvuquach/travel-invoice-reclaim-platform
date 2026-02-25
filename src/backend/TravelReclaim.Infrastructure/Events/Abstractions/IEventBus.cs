namespace TravelReclaim.Infrastructure.Events.Abstractions;

/// <summary>
/// Abstraction over the message broker. Backed by <c>RabbitMqEventBus</c> in production
/// and by a mock in unit tests — the Application layer never imports MassTransit directly.
/// </summary>
public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default) where TEvent : class;
}