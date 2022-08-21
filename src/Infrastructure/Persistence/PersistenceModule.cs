using Autofac;

using Domain.Core;
using Domain.Payments;

namespace Infrastructure.Persistence;

public class PersistenceModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<PaymentsDbContext>().SingleInstance();
        builder.RegisterType<DataRepository<Payment>>().As<IDataRepository<Payment>>();

        // Registration below is cleaner and generic. But needs some time to do tuning in other places.
        // builder.RegisterGeneric(typeof(DataRepository<>))
        //     .As(typeof(IDataRepository<>));
    }
}