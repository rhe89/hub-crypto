using System.Threading;
using System.Threading.Tasks;
using Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Crypto.Shared.Constants;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;

namespace Crypto.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCoinbaseAssetHistoryCommand : ServiceBusQueueCommand, ICommandWithConsumers
{
    private readonly IUpdateCoinbaseAssetHistoryCommandHandler _updateCoinbaseAssetHistoryCommandHandler;
    private readonly IMessageSender _messageSender;

    public UpdateCoinbaseAssetHistoryCommand(IUpdateCoinbaseAssetHistoryCommandHandler updateCoinbaseAssetHistoryCommandHandler,
        IMessageSender messageSender)
    {
        _updateCoinbaseAssetHistoryCommandHandler = updateCoinbaseAssetHistoryCommandHandler;
        _messageSender = messageSender;
    }
        
    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _updateCoinbaseAssetHistoryCommandHandler.UpdateAssetHistory();
    }

    public async Task NotifyConsumers()
    {
        await _messageSender.AddToQueue(QueueNames.CryptoAssetHistoryUpdated);
    }

    public override string Trigger => QueueNames.UpdateCoinbaseAssetHistory;
}