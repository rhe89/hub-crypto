using AutoMapper;

namespace Crypto.Data.AutoMapper;

public static class MapperConfigurationExpressionExtensions
{
    public static void AddEntityMappingProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
    {
        mapperConfigurationExpression.AddProfile<AccountMapperProfile>();
        mapperConfigurationExpression.AddProfile<AssetMapperProfile>();
        mapperConfigurationExpression.AddProfile<ExchangeRateMapperProfile>();
    }
}