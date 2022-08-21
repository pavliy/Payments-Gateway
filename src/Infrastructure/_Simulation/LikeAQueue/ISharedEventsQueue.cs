using ApplicationServices.Events;

using Infrastructure.Events;

namespace Infrastructure._Simulation.LikeAQueue;

public interface ISharedEventsQueue
{
    void Add(IntegrationEvent eventData);
}