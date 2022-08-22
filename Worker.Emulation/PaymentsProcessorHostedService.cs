using System.Threading.Channels;

using ApplicationServices.Events;
using ApplicationServices.PaymentsManagement.RemoteBanks;

using Domain.Core;
using Domain.Payments;

using Infrastructure.Persistence;
using Infrastructure.WebClients.RemoteBanks;

using Microsoft.Extensions.Hosting;

using Serilog;

namespace Worker.Emulation;

public class PaymentsProcessorHostedService : BackgroundService
{
    private readonly IBankOfUkClient bankOfUkClient;

    private readonly ChannelReader<IntegrationEvent> dataChannel;

    private readonly PaymentsDbContext paymentsDbContext;

    private readonly IDataRepository<Payment> paymentsRepository;

    private readonly ILogger logger;

    public PaymentsProcessorHostedService(
        ILogger logger,
        IDataRepository<Payment> paymentsRepository,
        PaymentsDbContext paymentsDbContext,
        ChannelReader<IntegrationEvent> dataChannel,
        IBankOfUkClient bankOfUkClient)
    {
        this.logger = logger;
        this.paymentsRepository = paymentsRepository;
        this.paymentsDbContext = paymentsDbContext;
        this.dataChannel = dataChannel;
        this.bankOfUkClient = bankOfUkClient;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (await this.dataChannel.WaitToReadAsync(cancellationToken))
        {
            while (this.dataChannel.TryRead(out IntegrationEvent? queueItem))
            {
                try
                {
                    if (queueItem is not PaymentCreatedEvent paymentCreated)
                    {
                        continue;
                    }

                    Payment? pendingPayment = await this.paymentsRepository.FindAsync(
                        paymentCreated.PaymentId,
                        cancellationToken,
                        false);
                    if (pendingPayment == null)
                    {
                        continue;
                    }

                    // Can also go to automapper
                    var payload = new BankOfUkWithdrawModel(
                        pendingPayment.Card.Number,
                        paymentCreated.CvvCode,
                        pendingPayment.Card.ExpireMonth,
                        pendingPayment.Card.ExpireYear,
                        pendingPayment.Spent.Amount,
                        pendingPayment.Spent.Currency);

                    try
                    {
                        await this.bankOfUkClient.WithdrawMoney(payload, cancellationToken);
                        pendingPayment.Approve();
                    }
                    catch (Exception ex)
                    {
                        this.logger.Error(ex, "Failed to process payment {Uuid}", pendingPayment.Uuid);
                        pendingPayment.Fail();
                    }

                    await this.paymentsDbContext.SaveEntitiesAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    this.logger.Error(e, "An unhandled exception occured");
                }
            }
        }
    }
}