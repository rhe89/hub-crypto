using System;
using AutoMapper;
using CoinbasePro.BackgroundTasks;
using CoinbasePro.Core.Integration;
using CoinbasePro.Data;
using CoinbasePro.Data.AutoMapper;
using CoinbasePro.Integration;
using Hub.HostedServices.Tasks;
using Hub.HostedServices.TimerHost;
using Hub.Storage.Repository.AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoinbasePro.BackgroundWorker
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactoryBase<CoinbaseProDbContext>
    {
        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddSingleton<IBackgroundTask, UpdateAccountsTask>();
            serviceCollection.TryAddSingleton<ICoinbaseProConnector, CoinbaseProConnector>();
            serviceCollection.AddHttpClient<ICoinbaseApiConnector, CoinbaseApiConnector>(client =>
                client.BaseAddress = new Uri(configuration.GetValue<string>("COINBASE_API_HOST")));
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddHostedServiceProfiles();
                c.AddCoinbaseProProfiles();
            });
        }
    }
}