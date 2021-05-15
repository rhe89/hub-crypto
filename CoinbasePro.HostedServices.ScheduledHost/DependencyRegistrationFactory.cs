using CoinbasePro.Data;
using CoinbasePro.HostedServices.ScheduledHost.Commands;
using Hub.HostedServices.Schedule;
using Hub.HostedServices.Schedule.Commands;
using Hub.ServiceBus;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoinbasePro.HostedServices.ScheduledHost
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactory<CoinbaseProDbContext>
    {
        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IMessageSender, MessageSender>();
            serviceCollection.AddSingleton<IScheduledCommand, QueueUpdateCoinbaseProAccountsCommand>();

        }
    }
}