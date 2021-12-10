using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinbasePro.Shared.Constants;
using CoinbasePro.Network.Authentication;
using CoinbasePro.Services.Accounts.Models;
using Microsoft.Extensions.Configuration;

namespace CoinbasePro.Integration
{
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
        
        private class CoinbaseProConnectorException : Exception
        {
            public CoinbaseProConnectorException(string message, Exception innerException) : base(message, innerException)
            {
            
            }
        }
    }
    
    
}
