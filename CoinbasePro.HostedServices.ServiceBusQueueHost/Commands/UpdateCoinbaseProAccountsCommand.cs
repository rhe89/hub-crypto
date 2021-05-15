using System.Threading;
using System.Threading.Tasks;
using CoinbasePro.Core.Constants;
using CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.HostedServices.Commands.Core;
using Hub.HostedServices.ServiceBusQueue.Commands;
using Hub.ServiceBus.Core;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.Commands
{
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
            await _messageSender.AddToQueue(QueueNames.CoinbaseProAccountsUpdated);
        }

        public override string QueueName => QueueNames.UpdateCoinbaseProAccounts;
    }
}