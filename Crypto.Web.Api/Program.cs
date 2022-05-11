using Crypto.Data;
using Crypto.Data.AutoMapper;
using Crypto.Providers;
using Hub.Shared.Web.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApiBuilder.CreateWebApplicationBuilder<CryptoDbContext>(args, "SQL_DB_CRYPTO");

builder.Services.TryAddTransient<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.TryAddTransient<IAccountProvider, AccountProvider>();
builder.Services.TryAddTransient<IAssetHistoryProvider, AssetHistoryProvider>();
builder.Services.AddAutoMapper(c =>
{
    c.AddEntityMappingProfiles();
});

builder
    .BuildApp()
    .Run();