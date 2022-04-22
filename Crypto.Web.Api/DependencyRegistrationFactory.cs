using Crypto.Data;
using Crypto.Data.AutoMapper;
using Crypto.Providers;
using Hub.Shared.Web.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Crypto.Web.Api;

public class DependencyRegistrationFactory : DependencyRegistrationFactory<CryptoDbContext>
{
    public DependencyRegistrationFactory() : base("SQL_DB_COINBASE_PRO", "Crypto.Data")
    {
    }

    protected override void AddDomainDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.TryAddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        serviceCollection.TryAddTransient<IAccountProvider, AccountProvider>();
        serviceCollection.TryAddTransient<IAssetHistoryProvider, AssetHistoryProvider>();
        serviceCollection.AddAutoMapper(c =>
        {
            c.AddEntityMappingProfiles();
        });
    }
}