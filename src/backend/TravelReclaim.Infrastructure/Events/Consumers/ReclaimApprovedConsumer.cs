using MassTransit;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Infrastructure.Events.Consumers;

public class ReclaimApprovedConsumer(IEventHandler<ReclaimApprovedEvent> handler)
    : IConsumer<ReclaimApprovedEvent>
{
    public Task Consume(ConsumeContext<ReclaimApprovedEvent> context) =>
        handler.HandleAsync(context.Message, context.CancellationToken);
}
