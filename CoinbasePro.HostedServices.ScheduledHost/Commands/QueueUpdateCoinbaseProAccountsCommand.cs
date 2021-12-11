using System.Threading;
using System.Threading.Tasks;
using CoinbasePro.Shared.Constants;
using Hub.Shared.HostedServices.Commands;
using Hub.Shared.HostedServices.Schedule;
using Hub.Shared.Storage.ServiceBus;

namespace CoinbasePro.HostedServices.ScheduledHost.Commands;

public class QueueUpdateCoinbaseProAccountsCommand : ScheduledCommand
{
    private readonly IMessageSender _messageSender;

    public QueueUpdateCoinbaseProAccountsCommand(ICommandConfigurationProvider commandConfigurationProvider,
        ICommandConfigurationFactory commandConfigurationFactory,
        IMessageSender messageSender) : base(commandConfigurationProvider, commandConfigurationFactory)
    {
        _messageSender = messageSender;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await _messageSender.AddToQueue(QueueNames.UpdateCoinbaseProAccounts);
    }

        
}