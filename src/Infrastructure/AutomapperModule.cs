using ApplicationServices;

using Autofac;

using AutoMapper.Contrib.Autofac.DependencyInjection;

namespace Infrastructure;

// ReSharper disable once UnusedType.Global
public class AutomapperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterAutoMapper(typeof(ApplicationServicesAssembly).Assembly);
    }
}