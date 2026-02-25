using MassTransit;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Infrastructure.Events.Consumers;

public class ReclaimProcessedConsumer(IEventHandler<ReclaimProcessedEvent> handler)
    : IConsumer<ReclaimProcessedEvent>
{
    public Task Consume(ConsumeContext<ReclaimProcessedEvent> context) =>
        handler.HandleAsync(context.Message, context.CancellationToken);
}
