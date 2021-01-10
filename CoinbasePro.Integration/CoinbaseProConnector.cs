using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinbasePro.Core.Constants;
using CoinbasePro.Core.Exceptions;
using CoinbasePro.Core.Integration;
using CoinbasePro.Network.Authentication;
using CoinbasePro.Services.Accounts.Models;
using Hub.Storage.Core.Providers;

namespace CoinbasePro.Integration
{
    public class CoinbaseProConnector : ICoinbaseProConnector
    {
        private readonly Authenticator _authenticator;

        public CoinbaseProConnector(ISettingProvider settingProvider)
        {
            var apiKey = settingProvider.GetSetting<string>(SettingConstants.CoinbaseProApiKey);
            var apiSecret = settingProvider.GetSetting<string>(SettingConstants.CoinbaseProApiSecret);
            var passPhrase = settingProvider.GetSetting<string>(SettingConstants.CoinbaseProPassphrase);
            
            _authenticator = new Authenticator(apiKey, apiSecret, passPhrase);
        }

        public async Task<IList<Account>> GetAccounts()
        {
            var coinbaseProClient = new CoinbaseProClient(_authenticator);

            try
            {
                var accounts = await coinbaseProClient.AccountsService.GetAllAccountsAsync();
                
                return accounts
                    .ToList();
            }
            catch (Exception e)
            {
                throw new CoinbaseProConnectorException("CoinbaseProConnector: Error sending request to Coinbase Pro API", e);
            }
        }
    }
}
