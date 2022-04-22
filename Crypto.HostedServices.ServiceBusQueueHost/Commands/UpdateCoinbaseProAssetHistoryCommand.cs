using System.Threading;
using System.Threading.Tasks;
using Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;

namespace Crypto.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCoinbaseProAssetHistoryCommand : ServiceBusQueueCommand, ICommandWithConsumers
{
    private readonly IUpdateCoinbaseProAssetHistoryCommandHandler _updateCoinbaseProAssetHistoryCommandHandler;
    private readonly IMessageSender _messageSender;

    public UpdateCoinbaseProAssetHistoryCommand(IUpdateCoinbaseProAssetHistoryCommandHandler updateCoinbaseProAssetHistoryCommandHandler,
        IMessageSender messageSender)
    {
        _updateCoinbaseProAssetHistoryCommandHandler = updateCoinbaseProAssetHistoryCommandHandler;
        _messageSender = messageSender;
    }
        
    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _updateCoinbaseProAssetHistoryCommandHandler.UpdateAccountBalance();
    }

    public async Task NotifyConsumers()
    {
        await _messageSender.AddToQueue(QueueNames.CryptoAssetHistoryUpdated);
    }

    public override string Trigger => QueueNames.UpdateCoinbaseProAssetHistory;
}