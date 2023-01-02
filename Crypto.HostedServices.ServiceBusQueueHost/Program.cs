using Crypto.Data;
using Crypto.Data.AutoMapper;
using Crypto.HostedServices.ServiceBusQueueHost.Commands;
using Crypto.HostedServices.ServiceBusQueueHost.QueueListenerServices;
using Crypto.Integration;
using Crypto.Providers;
using Crypto.Services;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

ServiceBusHostBuilder
    .CreateHostBuilder<CryptoDbContext>(args, "SQL_DB_CRYPTO")
    .ConfigureServices(serviceCollection =>
    {
        serviceCollection.AddSingleton<IMessageSender, MessageSender>();
        serviceCollection.AddSingleton<ICurrencyService, CurrencyService>();
        serviceCollection.AddSingleton<ICurrencyProvider, CurrencyProvider>();
        serviceCollection.AddSingleton<ICurrencyPriceProvider, CurrencyPriceProvider>();
        serviceCollection.AddSingleton<ICoinMarketCapConnector, CoinMarketCapConnector>();
            
        serviceCollection.AddAutoMapper(c =>
        {
            c.AddEntityMappingProfiles();
        });
        
        serviceCollection.AddSingleton<UpdateCurrencyPricesCommand>();
        serviceCollection.AddHostedService<UpdateCurrencyPricesQueueListener>();
    })
    .Build()
    .Run();