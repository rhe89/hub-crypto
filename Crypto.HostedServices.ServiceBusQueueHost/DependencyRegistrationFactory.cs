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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.HostedServices.ServiceBusQueueHost;

public class DependencyRegistrationFactory : DependencyRegistrationFactory<CryptoDbContext>
{
    protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IMessageSender, MessageSender>();
        serviceCollection.AddSingleton<IUpdateCoinbaseProAccountsCommandHandler, UpdateCoinbaseProAccountsCommandHandler>();
        serviceCollection.AddSingleton<IUpdateCoinbaseProAccountBalanceHistoryCommandHandler, UpdateCoinbaseProAccountBalanceHistoryCommandHandler>();
        serviceCollection.AddSingleton<IUpdateCoinbaseExchangeRatesCommandHandler, UpdateCoinbaseExchangeRatesCommandHandler>();
        serviceCollection.AddSingleton<IUpdateCoinbaseAccountsCommandHandler, UpdateCoinbaseAccountsCommandHandler>();
        serviceCollection.AddSingleton<IUpdateCoinbaseAccountBalanceHistoryCommandHandler, UpdateCoinbaseAccountBalanceHistoryCommandHandler>();
        
        serviceCollection.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
        serviceCollection.AddSingleton<IExchangeRateService, ExchangeRateService>();
        serviceCollection.AddSingleton<ICoinbaseProConnector, CoinbaseProConnector>();
        serviceCollection.AddSingleton<ICoinbaseConnector, CoinbaseConnector>();
            
        serviceCollection.AddAutoMapper(c =>
        {
            c.AddEntityMappingProfiles();
        });
    }

    protected override void AddQueueListenerServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddTransient<UpdateCoinbaseProAccountsCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseProAccountBalanceHistoryCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseAccountsCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseAccountBalanceHistoryCommand>();
        serviceCollection.AddTransient<UpdateCoinbaseExchangeRatesCommand>();  

        serviceCollection.AddHostedService<UpdateCoinbaseProAccountsQueueListener>();
        serviceCollection.AddHostedService<UpdateCoinbaseProAccountsBalanceHistoryQueueListener>();
        serviceCollection.AddHostedService<UpdateCoinbaseAccountsQueueListener>();
        serviceCollection.AddHostedService<UpdateCoinbaseAccountsBalanceHistoryQueueListener>();
        serviceCollection.AddHostedService<UpdateCoinbaseExchangeRatesQueueListener>();
    }
}