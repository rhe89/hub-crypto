using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinbasePro;
using Crypto.Shared.Constants;
using CoinbasePro.Network.Authentication;
using CoinbasePro.Services.Accounts.Models;
using Microsoft.Extensions.Configuration;

namespace Crypto.Integration;

public interface ICoinbaseProConnector
{
    Task<IList<Account>> GetAccounts();
}
    
public class CoinbaseProConnector : ICoinbaseProConnector
{
    private readonly Authenticator _authenticator;

    public CoinbaseProConnector(IConfiguration settingProvider)
    {
        var apiKey = settingProvider.GetValue<string>(SettingKeys.CoinbaseProApiKey);
        var apiSecret = settingProvider.GetValue<string>(SettingKeys.CoinbaseProApiSecret);
        var passPhrase = settingProvider.GetValue<string>(SettingKeys.CoinbaseProPassphrase);
            
        _authenticator = new Authenticator(apiKey, apiSecret, passPhrase);
    }

    public async Task<IList<Account>> GetAccounts()
    {
        var coinbaseProClient = new CoinbaseProClient(_authenticator);

        var accounts = await coinbaseProClient.AccountsService.GetAllAccountsAsync();
            
        return accounts.ToList();
    }
}