namespace ApplicationServices.Events;

public interface IEventsDispatcherService
{
    Task DispatchItemAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken);
}