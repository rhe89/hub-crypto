using Crypto.Data;
using Crypto.Data.AutoMapper;
using Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;
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
        serviceCollection.AddSingleton<IUpdateCoinbaseProAccountsCommandHandler, UpdateCoinbaseProAccountsCommandHandler>();
        serviceCollection.AddSingleton<IUpdateCoinbaseProAssetHistoryCommandHandler, UpdateCoinbaseProAssetHistoryCommandHandler>();
        serviceCollection.AddSingleton<IUpdateCoinbaseExchangeRatesCommandHandler, UpdateCoinbaseExchangeRatesCommandHandler>();
        serviceCollection.AddSingleton<IUpdateCoinbaseAccountsCommandHandler, UpdateCoinbaseAccountsCommandHandler>();
        serviceCollection.AddSingleton<IUpdateCoinbaseAssetHistoryCommandHandler, UpdateCoinbaseAssetHistoryCommandHandler>();
        
        serviceCollection.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
        serviceCollection.AddSingleton<IExchangeRateService, ExchangeRateService>();
        serviceCollection.AddSingleton<ICoinbaseProConnector, CoinbaseProConnector>();
        serviceCollection.AddSingleton<ICoinbaseConnector, CoinbaseConnector>();
            
        serviceCollection.AddAutoMapper(c =>
        {
            c.AddEntityMappingProfiles();
        });
        
        serviceCollection.AddTransient<UpdateCoinbaseProAccountsCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseProAssetHistoryCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseAccountsCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseAssetHistoryCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseExchangeRatesCommand>();  

        serviceCollection.AddHostedService<UpdateCoinbaseProAccountsQueueListener>();
        serviceCollection.AddHostedService<UpdateCoinbaseProAssetHistoryQueueListener>();
        serviceCollection.AddHostedService<UpdateCoinbaseAccountsQueueListener>();
        serviceCollection.AddHostedService<UpdateCoinbaseAssetHistoryQueueListener>();
        serviceCollection.AddHostedService<UpdateCoinbaseExchangeRatesQueueListener>();
    })
    .Build()
    .Run();