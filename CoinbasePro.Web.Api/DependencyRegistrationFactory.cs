using System;
using AutoMapper;
using Coinbase.Core.Providers;
using CoinbasePro.BackgroundTasks;
using CoinbasePro.Core.Integration;
using CoinbasePro.Data;
using CoinbasePro.Data.AutoMapper;
using CoinbasePro.Integration;
using CoinbasePro.Providers;
using CoinbasePro.Web.Api.Services;
using Hub.Storage.Repository.AutoMapper;
using Hub.Web.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoinbasePro.Web.Api
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactoryWithHostedServiceBase<CoinbaseProDbContext>
    {
        public DependencyRegistrationFactory() : base("SQL_DB_COINBASE_PRO", "CoinbasePro.Data")
        {
        }

        protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddTransient<IAccountProvider, AccountProvider>();
            serviceCollection.TryAddTransient<IAssetsProvider, AssetsProvider>();
            serviceCollection.TryAddTransient<IAssetsService, AssetsService>();
            serviceCollection.TryAddTransient<IAccountService, AccountService>();
            serviceCollection.TryAddScoped<UpdateAccountsTask>();
            serviceCollection.TryAddScoped<ICoinbaseProConnector, CoinbaseProConnector>();
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