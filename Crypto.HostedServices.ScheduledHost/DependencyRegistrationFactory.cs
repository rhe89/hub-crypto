using Crypto.Data;
using Crypto.HostedServices.ScheduledHost.Commands;
using Hub.Shared.HostedServices.Schedule;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.HostedServices.ScheduledHost;

public class DependencyRegistrationFactory : DependencyRegistrationFactory<CryptoDbContext>
{
    protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IMessageSender, MessageSender>();
        serviceCollection.AddSingleton<IScheduledCommand, QueueUpdateCoinbaseExchangeRatesCommand>();

    }
}