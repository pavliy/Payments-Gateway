using Autofac;

using Infrastructure.Serialization;

namespace Infrastructure.WebClients.RemoteBanks;

// ReSharper disable once UnusedType.Global
public class RemoteBanksModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.Register<IBankOfUkClient>(
            ctx =>
                {
                    var clientFactory = ctx.Resolve<IHttpClientFactory>();
                    HttpClient httpClient = clientFactory.CreateClient(HttpClientRegistrations.BankOfUkHttpClient);

                    var serializer = ctx.Resolve<IJsonSerializer>();

                    return new BankOfUkClient(httpClient, serializer);
                });
    }
}