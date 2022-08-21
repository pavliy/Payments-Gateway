using System.Diagnostics.CodeAnalysis;

using Autofac;

namespace Infrastructure.Serialization;

[ExcludeFromCodeCoverage]

// ReSharper disable once UnusedType.Global
public class SerializationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterType<NewtonsoftJsonSerializer>().As<IJsonSerializer>().SingleInstance();
    }
}