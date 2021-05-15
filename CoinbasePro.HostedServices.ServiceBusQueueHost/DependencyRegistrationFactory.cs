using System;
using AutoMapper;
using CoinbasePro.Core.Integration;
using CoinbasePro.Data;
using CoinbasePro.Data.AutoMapper;
using CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers;
using CoinbasePro.HostedServices.ServiceBusQueueHost.Commands;
using CoinbasePro.HostedServices.ServiceBusQueueHost.QueueListenerServices;
using CoinbasePro.Integration;
using Hub.HostedServices.ServiceBusQueue;
using Hub.ServiceBus;
using Hub.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactory<CoinbaseProDbContext>
    {
        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IMessageSender, MessageSender>();
            serviceCollection.AddSingleton<IUpdateCoinbaseProAccountsCommandHandler, UpdateCoinbaseProAccountsCommandHandler>();
            serviceCollection.AddSingleton<ICoinbaseProConnector, CoinbaseProConnector>();
            serviceCollection.AddHttpClient<ICoinbaseApiConnector, CoinbaseApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("COINBASE_API_HOST")));
            
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddCoinbaseProProfiles();
            });
        }

        protected override void AddQueueListenerServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<UpdateCoinbaseProAccountsCommand>();
            
            serviceCollection.AddHostedService<UpdateCoinbaseProAccountsQueueListener>();
        }
    }
}