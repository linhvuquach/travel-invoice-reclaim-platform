using MassTransit;
using TravelReclaim.Infrastructure.Events.Abstractions;
using TravelReclaim.Infrastructure.Events.Models;

namespace TravelReclaim.Infrastructure.Events.Consumers;

public class InvoiceCreatedConsumer(IEventHandler<InvoiceCreatedEvent> handler) : IConsumer<InvoiceCreatedEvent>
{
    public Task Consume(ConsumeContext<InvoiceCreatedEvent> context) =>
        handler.HandleAsync(context.Message, context.CancellationToken);
}