using CoinbasePro.Data;
using CoinbasePro.HostedServices.ScheduledHost.Commands;
using Hub.Shared.HostedServices.Schedule;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoinbasePro.HostedServices.ScheduledHost;

public class DependencyRegistrationFactory : DependencyRegistrationFactory<CoinbaseProDbContext>
{
    protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IMessageSender, MessageSender>();
        serviceCollection.AddSingleton<IScheduledCommand, QueueUpdateCoinbaseProAccountsCommand>();

    }
}