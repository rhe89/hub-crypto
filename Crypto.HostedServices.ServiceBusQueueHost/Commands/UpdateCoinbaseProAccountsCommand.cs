using System.Threading;
using System.Threading.Tasks;
using Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;

namespace Crypto.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCoinbaseProAccountsCommand : ServiceBusQueueCommand, ICommandWithConsumers
{
    private readonly IUpdateCoinbaseProAccountsCommandHandler _updateCoinbaseProAccountsCommandHandler;
    private readonly IMessageSender _messageSender;

    public UpdateCoinbaseProAccountsCommand(IUpdateCoinbaseProAccountsCommandHandler updateCoinbaseProAccountsCommandHandler,
        IMessageSender messageSender)
    {
        _updateCoinbaseProAccountsCommandHandler = updateCoinbaseProAccountsCommandHandler;
        _messageSender = messageSender;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _updateCoinbaseProAccountsCommandHandler.UpdateAccountAssets();
    }

    public async Task NotifyConsumers()
    {
        await _messageSender.AddToQueue(QueueNames.CryptoAccountsUpdated);
        await _messageSender.AddToQueue(QueueNames.UpdateCoinbaseProAssetHistory);
    }

    public override string Trigger => QueueNames.UpdateCoinbaseProAccounts;
}