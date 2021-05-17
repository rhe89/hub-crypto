using CoinbasePro.HostedServices.ServiceBusQueueHost.Commands;
using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.QueueListenerServices
{
    public class UpdateCoinbaseAccountsBalanceHistoryQueueListener : ServiceBusHostedService
    {
        public UpdateCoinbaseAccountsBalanceHistoryQueueListener(ILogger<UpdateCoinbaseAccountsBalanceHistoryQueueListener> logger, 
            ICommandLogFactory commandLogFactory, 
            IConfiguration configuration,
            UpdateCoinbaseProAccountBalanceHistoryCommand command, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                 commandLogFactory, 
                                                 configuration,
                                                 command, 
                                                 queueProcessor)
        {
        }
    }
}