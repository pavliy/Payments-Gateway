using ApplicationServices;

using Autofac;

namespace Infrastructure;

public class ValidationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterAssemblyTypes(typeof(ApplicationServicesAssembly).Assembly)
            .Where(t => t.Name.EndsWith("Validator"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        base.Load(builder);
    }
}