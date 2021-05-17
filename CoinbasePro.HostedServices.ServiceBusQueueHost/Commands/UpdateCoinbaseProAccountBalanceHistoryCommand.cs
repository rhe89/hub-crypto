using System.Threading;
using System.Threading.Tasks;
using CoinbasePro.Core.Constants;
using CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers;
using Hub.HostedServices.Commands.Core;
using Hub.HostedServices.ServiceBusQueue.Commands;
using Hub.ServiceBus.Core;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.Commands
{
    public class UpdateCoinbaseProAccountBalanceHistoryCommand : ServiceBusQueueCommand, ICommandWithConsumers
    {
        private readonly IUpdateCoinbaseProAccountBalanceHistoryCommandHandler _updateCoinbaseProAccountBalanceHistoryCommandHandler;
        private readonly IMessageSender _messageSender;

        public UpdateCoinbaseProAccountBalanceHistoryCommand(IUpdateCoinbaseProAccountBalanceHistoryCommandHandler updateCoinbaseProAccountBalanceHistoryCommandHandler,
            IMessageSender messageSender)
        {
            _updateCoinbaseProAccountBalanceHistoryCommandHandler = updateCoinbaseProAccountBalanceHistoryCommandHandler;
            _messageSender = messageSender;
        }
        
        public override async Task Execute(CancellationToken cancellationToken)
        {
            await _updateCoinbaseProAccountBalanceHistoryCommandHandler.UpdateAccountBalance();
        }

        public async Task NotifyConsumers()
        {
            await _messageSender.AddToQueue(QueueNames.CoinbaseProAccountBalanceHistoryUpdated);
        }

        public override string QueueName => QueueNames.UpdateCoinbaseProAccountBalances;
    }
}