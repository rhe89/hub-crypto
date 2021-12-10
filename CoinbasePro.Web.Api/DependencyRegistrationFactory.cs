using AutoMapper;
using CoinbasePro.Data;
using CoinbasePro.Data.AutoMapper;
using CoinbasePro.Providers;
using Hub.Shared.Web.Api;
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
            serviceCollection.TryAddTransient<IAccountBalanceProvider, AccountBalanceProvider>();
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddCoinbaseProProfiles();
            });
        }
    }
}