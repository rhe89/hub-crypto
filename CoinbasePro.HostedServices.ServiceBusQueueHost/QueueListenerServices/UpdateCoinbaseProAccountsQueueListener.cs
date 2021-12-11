using CoinbasePro.HostedServices.ServiceBusQueueHost.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.QueueListenerServices;

public class UpdateCoinbaseProAccountsQueueListener : ServiceBusHostedService
{
    public UpdateCoinbaseProAccountsQueueListener(ILogger<UpdateCoinbaseProAccountsQueueListener> logger, 
        IConfiguration configuration,
        UpdateCoinbaseProAccountsCommand command, 
        IQueueProcessor queueProcessor,
        TelemetryClient telemetryClient) : base(logger, 
        configuration,
        command, 
        queueProcessor,
        telemetryClient)
    {
    }
}