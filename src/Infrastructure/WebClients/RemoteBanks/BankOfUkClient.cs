using ApplicationServices.PaymentsManagement.RemoteBanks;

using Infrastructure.Serialization;

namespace Infrastructure.WebClients.RemoteBanks;

public class BankOfUkClient : ApiClientBase, IBankOfUkClient
{
    public BankOfUkClient(
        HttpClient httpClient,
        IJsonSerializer jsonSerializer)
        : base(jsonSerializer, httpClient)
    {
    }

    public async Task WithdrawMoney(BankOfUkWithdrawModel payload, CancellationToken cancellationToken)
    {
        await this.SendInternal<HttpResponseMessage>("bankOfUk", payload, HttpMethod.Post, cancellationToken);
    }
}