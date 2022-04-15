using System.Threading;
using System.Threading.Tasks;
using Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Crypto.Shared.Constants;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;

namespace Crypto.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCoinbaseAccountBalanceHistoryCommand : ServiceBusQueueCommand, ICommandWithConsumers
{
    private readonly IUpdateCoinbaseAccountBalanceHistoryCommandHandler _updateCoinbaseAccountBalanceHistoryCommandHandler;
    private readonly IMessageSender _messageSender;

    public UpdateCoinbaseAccountBalanceHistoryCommand(IUpdateCoinbaseAccountBalanceHistoryCommandHandler updateCoinbaseAccountBalanceHistoryCommandHandler,
        IMessageSender messageSender)
    {
        _updateCoinbaseAccountBalanceHistoryCommandHandler = updateCoinbaseAccountBalanceHistoryCommandHandler;
        _messageSender = messageSender;
    }
        
    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _updateCoinbaseAccountBalanceHistoryCommandHandler.UpdateAccountBalance();
    }

    public async Task NotifyConsumers()
    {
        await _messageSender.AddToQueue(QueueNames.CoinbaseAccountBalanceHistoryUpdated);
    }

    public override string Trigger => QueueNames.UpdateCoinbaseAccountBalances;
}