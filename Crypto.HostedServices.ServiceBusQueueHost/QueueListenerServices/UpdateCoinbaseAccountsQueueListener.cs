using Crypto.HostedServices.ServiceBusQueueHost.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Crypto.HostedServices.ServiceBusQueueHost.QueueListenerServices;

public class UpdateCoinbaseAccountsQueueListener : ServiceBusHostedService
{
    public UpdateCoinbaseAccountsQueueListener(ILogger<UpdateCoinbaseAccountsQueueListener> logger, 
        IConfiguration configuration,
        UpdateCoinbaseAccountsCommand command, 
        IQueueProcessor queueProcessor,
        TelemetryClient telemetryClient) : base(logger, 
        configuration,
        command, 
        queueProcessor,
        telemetryClient)
    {
    }
}