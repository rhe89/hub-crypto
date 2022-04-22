using System.Threading;
using System.Threading.Tasks;
using Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Crypto.Shared.Constants;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;

namespace Crypto.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCoinbaseAccountsCommand : ServiceBusQueueCommand, ICommandWithConsumers
{
    private readonly IUpdateCoinbaseAccountsCommandHandler _updateCoinbaseAccountsCommandHandler;
    private readonly IMessageSender _messageSender;

    public UpdateCoinbaseAccountsCommand(IUpdateCoinbaseAccountsCommandHandler updateCoinbaseAccountsCommandHandler,
        IMessageSender messageSender)
    {
        _updateCoinbaseAccountsCommandHandler = updateCoinbaseAccountsCommandHandler;
        _messageSender = messageSender;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _updateCoinbaseAccountsCommandHandler.UpdateAccounts();
    }

    public async Task NotifyConsumers()
    {
        await _messageSender.AddToQueue(QueueNames.CryptoAccountsUpdated);
        await _messageSender.AddToQueue(QueueNames.UpdateCoinbaseAssetHistory);
    }

    public override string Trigger => QueueNames.UpdateCoinbaseAccounts;
}