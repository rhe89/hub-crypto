using CoinbasePro.HostedServices.ServiceBusQueueHost.Commands;
using Hub.HostedServices.Commands.Logging.Core;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.QueueListenerServices
{
    public class UpdateCoinbaseProAccountsQueueListener : ServiceBusHostedService
    {
        public UpdateCoinbaseProAccountsQueueListener(ILogger<UpdateCoinbaseProAccountsQueueListener> logger, 
            ICommandLogFactory commandLogFactory, 
            IConfiguration configuration,
            UpdateCoinbaseProAccountsCommand command, 
            IQueueProcessor queueProcessor) : base(logger, 
                                                 commandLogFactory, 
                                                 configuration,
                                                 command, 
                                                 queueProcessor)
        {
        }
    }
}