using System.Threading;
using System.Threading.Tasks;
using CoinbasePro.Core.Constants;
using Hub.HostedServices.Commands.Configuration.Core;
using Hub.HostedServices.Schedule.Commands;
using Hub.ServiceBus.Core;

namespace CoinbasePro.HostedServices.ScheduledHost.Commands
{
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
}