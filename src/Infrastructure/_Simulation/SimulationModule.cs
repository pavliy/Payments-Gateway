using Autofac;

using Infrastructure._Simulation.LikeAQueue;

namespace Infrastructure._Simulation;

public class SimulationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<SharedEventsQueue>().AsSelf().As<ISharedEventsQueue>().SingleInstance();
    }
}