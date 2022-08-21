using System.Net;

using ApplicationServices.PaymentsManagement.RemoteBanks;

namespace Infrastructure.WebClients.RemoteBanks;

public interface IBankOfUkClient
{
    Task WithdrawMoney(BankOfUkWithdrawModel payload, CancellationToken cancellationToken);
}