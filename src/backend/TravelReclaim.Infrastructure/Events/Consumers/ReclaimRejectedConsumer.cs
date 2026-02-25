using MassTransit;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Infrastructure.Events.Consumers;

public class ReclaimRejectedConsumer(IEventHandler<ReclaimRejectedEvent> handler)
    : IConsumer<ReclaimRejectedEvent>
{
    public Task Consume(ConsumeContext<ReclaimRejectedEvent> context) =>
        handler.HandleAsync(context.Message, context.CancellationToken);
}
