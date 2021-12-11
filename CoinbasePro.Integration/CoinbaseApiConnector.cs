using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Hub.Shared.DataContracts.Coinbase;
using Hub.Shared.Web.Http;

namespace CoinbasePro.Integration;

public interface ICoinbaseApiConnector
{
    Task<IList<ExchangeRateDto>> GetExchangeRates();
    Task<ExchangeRateDto> GetExchangeRate(string currency);
}
    
public class CoinbaseApiConnector : HttpClientService, ICoinbaseApiConnector
{
    private const string ExchangeRatesPath = "/api/exchangerates";
        
    public CoinbaseApiConnector(HttpClient httpClient) : base(httpClient, "CoinbaseApi") {}
        
    public async Task<IList<ExchangeRateDto>> GetExchangeRates()
    {
        return await Get<IList<ExchangeRateDto>>(ExchangeRatesPath);
    }

    public async Task<ExchangeRateDto> GetExchangeRate(string currency)
    {
        return await Get<ExchangeRateDto>(ExchangeRatesPath, $"currency={currency}");
    }
}