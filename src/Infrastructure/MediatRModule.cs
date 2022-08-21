using ApplicationServices;

using Autofac;
using Autofac.Core;

using MediatR;
using MediatR.Pipeline;

namespace Infrastructure;

// ReSharper disable once UnusedType.Global
public class MediatRModule : Module
{
    private const string RequestHandlerKey = "requestHandler";

    private static readonly Dictionary<string, string> ResolveKeysMap = new()
        {
            { typeof(IRequestHandler<,>).Name, MediatRModule.RequestHandlerKey }
        };

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
        builder.RegisterAssemblyTypes(typeof(ApplicationServicesAssembly).Assembly)
            .As(
                type => type.GetInterfaces()
                    .Select(
                        t => t.IsClosedTypeOf(typeof(IRequestHandler<,>))
                            ? new KeyedService(MediatRModule.RequestHandlerKey, t)
                            : null).Where(i => i != null)!).InstancePerLifetimeScope();

        builder.Register<ServiceFactory>(
            ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t =>
                        {
                            MediatRModule.ResolveKeysMap.TryGetValue(t.Name, out string? resolveKey);
                            return string.IsNullOrEmpty(resolveKey) ? c.Resolve(t) : c.ResolveKeyed(resolveKey, t);
                        };
                });
    }
}