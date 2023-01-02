using System.Collections.Generic;
using System.Linq;
using Crypto.Shared.Constants;
using Microsoft.Extensions.Configuration;
using NoobsMuc.Coinmarketcap.Client;

namespace Crypto.Integration;

public interface ICoinMarketCapConnector
{
    IList<Currency> GetCurrencies(string[] currencies);
}

public class CoinMarketCapConnector : ICoinMarketCapConnector
{
    private readonly CoinmarketcapClient _coinmarketcapClient;

    public CoinMarketCapConnector(IConfiguration configuration)
    {
        var apiSecret = configuration.GetValue<string>(SettingKeys.CoinMarketCapApiKey);
        _coinmarketcapClient = new CoinmarketcapClient(apiSecret);
    }

    public IList<Currency> GetCurrencies(string[] currencies)
    {
        var response = _coinmarketcapClient.GetCurrencyBySymbolList(currencies);

        return response.ToList();
    }
}