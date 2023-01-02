using System.Threading;
using System.Threading.Tasks;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.Schedule;
using Hub.Shared.Storage.ServiceBus;

namespace Crypto.HostedServices.ScheduledHost.Commands;

public class QueueUpdateUpdateCurrencyPricesCommand : ScheduledCommand
{
    private readonly IMessageSender _messageSender;

    public QueueUpdateUpdateCurrencyPricesCommand(ICommandConfigurationProvider commandConfigurationProvider,
        ICommandConfigurationFactory commandConfigurationFactory,
        IMessageSender messageSender) : base(commandConfigurationProvider, commandConfigurationFactory)
    {
        _messageSender = messageSender;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _messageSender.AddToQueue(QueueNames.UpdateCurrencyPrices);
    }

        
}