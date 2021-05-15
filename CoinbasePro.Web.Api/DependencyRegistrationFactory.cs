using AutoMapper;
using Coinbase.Core.Providers;
using CoinbasePro.Data;
using CoinbasePro.Data.AutoMapper;
using CoinbasePro.Providers;
using CoinbasePro.Web.Api.Services;
using Hub.Web.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoinbasePro.Web.Api
{
    public class DependencyRegistrationFactory : DependencyRegistrationFactory<CoinbaseProDbContext>
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
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddCoinbaseProProfiles();
            });
        }
    }
}