using Autofac;

namespace Infrastructure.Tools;

// ReSharper disable once UnusedType.Global
public class ToolsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>();
        builder.RegisterType<SampleCorrelationInfoAccessor>().As<ICorrelationInfoAccessor>();
    }
}