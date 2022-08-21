using ApplicationServices.Events;

using Autofac;

namespace Infrastructure.Events;

public class EventsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<EventsDispatcherService>().As<IEventsDispatcherService>().InstancePerLifetimeScope();
    }
}