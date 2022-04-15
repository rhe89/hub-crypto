using System.Threading;
using System.Threading.Tasks;
using Crypto.Shared.Constants;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.Schedule;
using Hub.Shared.Storage.ServiceBus;

namespace Crypto.HostedServices.ScheduledHost.Commands;

public class QueueUpdateCoinbaseExchangeRatesCommand : ScheduledCommand
{
    private readonly IMessageSender _messageSender;

    public QueueUpdateCoinbaseExchangeRatesCommand(ICommandConfigurationProvider commandConfigurationProvider,
        ICommandConfigurationFactory commandConfigurationFactory,
        IMessageSender messageSender) : base(commandConfigurationProvider, commandConfigurationFactory)
    {
        _messageSender = messageSender;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _messageSender.AddToQueue(QueueNames.UpdateCoinbaseExchangeRates);
    }

        
}