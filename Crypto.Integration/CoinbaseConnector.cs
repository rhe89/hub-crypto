using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinbase;
using Coinbase.Models;
using Crypto.Shared.Constants;
using Microsoft.Extensions.Configuration;

namespace Crypto.Integration;

public interface ICoinbaseConnector
{
    Task<IList<Account>> GetAccounts();
    Task<ExchangeRates> GetExchangeRatesForCurrency(string currency);
}
    
public class CoinbaseConnector : ICoinbaseConnector
{
    private readonly CoinbaseClient _coinbaseClient;

    public CoinbaseConnector(IConfiguration configuration)
    {
        var apiKey = configuration.GetValue<string>(SettingKeys.CoinbaseApiKey);
        var apiSecret = configuration.GetValue<string>(SettingKeys.CoinbaseApiSecret);
        _coinbaseClient = new CoinbaseClient(new ApiKeyConfig { ApiKey =  apiKey, ApiSecret = apiSecret});
    }
        
    public async Task<IList<Account>> GetAccounts()
    {
        var response = await _coinbaseClient.Accounts.ListAccountsAsync();

        ValidateResponse(response);
            
        if (response?.Data == null ||
            !response.Data.Any())
        {
            return null;
        }

        return response.Data.ToList();
    }
        
    public async Task<ExchangeRates> GetExchangeRatesForCurrency(string currency)
    {
        var response = await _coinbaseClient.Data.GetExchangeRatesAsync(currency);
            
        ValidateResponse(response);

        return response.Data;
    }
        
    private static void ValidateResponse(JsonResponse response)
    {
        if (response?.Errors == null || !response.Errors.Any() || response.Errors.Any(x => x.Id == "not_found"))
        {
            return;
        }
            
        var errors = response.Errors;

        var msg = string.Join(",", errors.Select(x => x.Message));

        throw new CoinbaseApiConnectorException($"CoinbaseConnector: Response from Coinbase contained errors: {msg}");
            
    }
}
    
public class CoinbaseApiConnectorException : Exception
{
    public CoinbaseApiConnectorException(string message) : base(message)
    {
    }
}