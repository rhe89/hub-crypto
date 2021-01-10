using AutoMapper;

namespace CoinbasePro.Data.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static IMapperConfigurationExpression AddCoinbaseProProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<AccountMapperProfile>();
            mapperConfigurationExpression.AddProfile<AssetsMapperProfile>();

            return mapperConfigurationExpression;
        }
    }
}