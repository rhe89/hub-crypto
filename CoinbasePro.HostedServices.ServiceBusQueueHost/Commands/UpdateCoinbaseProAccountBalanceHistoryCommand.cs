using System.Threading;
using System.Threading.Tasks;
using CoinbasePro.Shared.Constants;
using CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCoinbaseProAccountBalanceHistoryCommand : ServiceBusQueueCommand, ICommandWithConsumers
{
    private readonly IUpdateCoinbaseProAccountBalanceHistoryCommandHandler _updateCoinbaseProAccountBalanceHistoryCommandHandler;
    private readonly IMessageSender _messageSender;

    public UpdateCoinbaseProAccountBalanceHistoryCommand(IUpdateCoinbaseProAccountBalanceHistoryCommandHandler updateCoinbaseProAccountBalanceHistoryCommandHandler,
        IMessageSender messageSender)
    {
        _updateCoinbaseProAccountBalanceHistoryCommandHandler = updateCoinbaseProAccountBalanceHistoryCommandHandler;
        _messageSender = messageSender;
    }
        
    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _updateCoinbaseProAccountBalanceHistoryCommandHandler.UpdateAccountBalance();
    }

    public async Task NotifyConsumers()
    {
        await _messageSender.AddToQueue(QueueNames.CoinbaseProAccountBalanceHistoryUpdated);
    }

    public override string Trigger => QueueNames.UpdateCoinbaseProAccountBalances;
}