using AutoMapper;

namespace CoinbasePro.Data.AutoMapper
{
    public static class MapperConfigurationExpressionExtensions
    {
        public static void AddCoinbaseProProfiles(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<AccountMapperProfile>();
            mapperConfigurationExpression.AddProfile<AccountBalanceMapperProfile>();
        }
    }
}