namespace TravelReclaim.Infrastructure.Events.Abstractions;

/// <summary>
/// Handles a strongly-typed domain event.
/// Register implementations in DI; MassTransit consumers delegate to these handlers,
/// keeping the Application layer free of any broker dependency.
/// </summary>
public interface IEventHandler<TEvent> where TEvent : class
{
    Task HandleAsync(TEvent @event, CancellationToken ct = default);
}