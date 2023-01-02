using AutoMapper;

namespace Crypto.Data.AutoMapper;

public static class MapperConfigurationExpressionExtensions
{
    public static void AddEntityMappingProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
    {
        mapperConfigurationExpression.AddProfile<AccountMapperProfile>();
        mapperConfigurationExpression.AddProfile<AccountBalanceMapperProfile>();
        mapperConfigurationExpression.AddProfile<CurrencyMapperProfile>();
        mapperConfigurationExpression.AddProfile<CurrencyPriceMapperProfile>();
        mapperConfigurationExpression.AddProfile<WalletMapperProfile>();
    }
}