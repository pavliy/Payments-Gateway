using ApplicationServices.Events;

using Infrastructure.Events;

namespace Infrastructure._Simulation.LikeAQueue;

/// <summary>
///     NOTE! This is used only for DEMO purposes.
///     In real life that will be RabbitMQ, Azure service bus or different reliable mechanism
/// </summary>
public class SharedEventsQueue
{
    private readonly Queue<IntegrationEvent> events = new();

    public void Add(IntegrationEvent eventData)
    {
        this.events.Enqueue(eventData);
    }
}