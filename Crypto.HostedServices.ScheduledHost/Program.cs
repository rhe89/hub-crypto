using Crypto.Data;
using Crypto.HostedServices.ScheduledHost.Commands;
using Hub.Shared.HostedServices.Schedule;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

ScheduledHostBuilder
    .CreateHostBuilder<CryptoDbContext>(args, "SQL_DB_CRYPTO")
    .ConfigureServices(serviceCollection =>
    {
        serviceCollection.AddSingleton<IMessageSender, MessageSender>();
        serviceCollection.AddSingleton<IScheduledCommand, QueueUpdateCoinbaseExchangeRatesCommand>();
    })
    .Build()
    .Run();