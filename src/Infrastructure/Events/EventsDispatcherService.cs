using System.Threading.Channels;

using ApplicationServices.Events;

namespace Infrastructure.Events;

public class EventsDispatcherService : IEventsDispatcherService
{
    private readonly ChannelWriter<IntegrationEvent> eventsQueue;

    public EventsDispatcherService(ChannelWriter<IntegrationEvent> eventsQueue)
    {
        this.eventsQueue = eventsQueue;
    }

    public async Task DispatchItemAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        await this.eventsQueue.WriteAsync(integrationEvent, cancellationToken);
    }
}