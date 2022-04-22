using System.Threading;
using System.Threading.Tasks;
using Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;

namespace Crypto.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCoinbaseExchangeRatesCommand : ServiceBusQueueCommand, ICommandWithConsumers
{
    private readonly IUpdateCoinbaseExchangeRatesCommandHandler _updateCoinbaseExchangeRatesCommandHandler;
    private readonly IMessageSender _messageSender;

    public UpdateCoinbaseExchangeRatesCommand(IUpdateCoinbaseExchangeRatesCommandHandler updateCoinbaseExchangeRatesCommandHandler,
        IMessageSender messageSender)
    {
        _updateCoinbaseExchangeRatesCommandHandler = updateCoinbaseExchangeRatesCommandHandler;
        _messageSender = messageSender;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _updateCoinbaseExchangeRatesCommandHandler.UpdateExchangeRates();
    }

    public async Task NotifyConsumers()
    {
        await _messageSender.AddToQueue(QueueNames.ExchangeRatesUpdated);
        await _messageSender.AddToQueue(QueueNames.UpdateCoinbaseProAccounts);
        await _messageSender.AddToQueue(QueueNames.UpdateCoinbaseAccounts);
    }

    public override string Trigger => QueueNames.UpdateCoinbaseExchangeRates;
}