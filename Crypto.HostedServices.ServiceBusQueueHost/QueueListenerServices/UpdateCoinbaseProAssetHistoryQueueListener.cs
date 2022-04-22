using Crypto.HostedServices.ServiceBusQueueHost.Commands;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Crypto.HostedServices.ServiceBusQueueHost.QueueListenerServices;

public class UpdateCoinbaseProAssetHistoryQueueListener : ServiceBusHostedService
{
    public UpdateCoinbaseProAssetHistoryQueueListener(ILogger<UpdateCoinbaseProAssetHistoryQueueListener> logger, 
        IConfiguration configuration,
        UpdateCoinbaseProAssetHistoryCommand command, 
        IQueueProcessor queueProcessor,
        TelemetryClient telemetryClient) : base(logger, 
        configuration,
        command, 
        queueProcessor,
        telemetryClient)
    {
    }
}