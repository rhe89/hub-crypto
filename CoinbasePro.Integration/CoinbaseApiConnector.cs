using System.Net.Http;
using System.Threading.Tasks;
using CoinbasePro.Core.Dto.Integration;
using CoinbasePro.Core.Integration;
using Hub.Web.Http;

namespace CoinbasePro.Integration
{
    public class CoinbaseApiConnector : HttpClientService, ICoinbaseApiConnector
    {
        private const string ExchangeRatesPath = "/api/exchangerates/exchangerates";
        
        public CoinbaseApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseApi") {}
        
        public async Task<Response<ExchangeRatesDto>> GetExchangeRates(string currency)
        {
            return await Get<ExchangeRatesDto>(ExchangeRatesPath, $"currency={currency}");
        }
    }
}