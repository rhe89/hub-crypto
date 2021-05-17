using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoinbasePro.Core.Dto.Data;
using CoinbasePro.Core.Dto.Integration;
using CoinbasePro.Core.Entities;
using CoinbasePro.Core.Exceptions;
using CoinbasePro.Core.Integration;
using Hub.Storage.Repository.Core;
using Hub.Web.Http;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers
{
    public class UpdateCoinbaseProAccountsCommandHandler : IUpdateCoinbaseProAccountsCommandHandler
    {
        private readonly ILogger<UpdateCoinbaseProAccountsCommandHandler> _logger;
        private readonly ICoinbaseProConnector _coinbaseProConnector;
        private readonly ICoinbaseApiConnector _coinbaseApiConnector;
        private readonly IHubDbRepository _dbRepository;

        public UpdateCoinbaseProAccountsCommandHandler(ILogger<UpdateCoinbaseProAccountsCommandHandler> logger,
            ICoinbaseProConnector coinbaseProConnector,
            ICoinbaseApiConnector coinbaseApiConnector,
            IHubDbRepository dbRepository)
        {
            _logger = logger;
            _coinbaseProConnector = coinbaseProConnector;
            _coinbaseApiConnector = coinbaseApiConnector;
            _dbRepository = dbRepository;
        }
        
        public async Task UpdateAccountAssets()
        {
            var accountsInDb = await _dbRepository.AllAsync<Account, AccountDto>();
            
            var coinbaseProAccounts = await _coinbaseProConnector.GetAccounts();

            var exchangeRates = await GetExchangeRates();

            var accountsCount = accountsInDb.Count;

            var counter = 1;

            foreach (var dbAccount in accountsInDb)
            {
                _logger.LogInformation($"Updating account {counter++} of {accountsCount}: {dbAccount.Name}.");

                try
                {
                    await UpdateAccount(dbAccount, coinbaseProAccounts, exchangeRates);
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Failed updating account {dbAccount.Name}. Continuing", e.Message);
                }
            }

            await _dbRepository.ExecuteQueueAsync();

            _logger.LogInformation("Done updating cryptocurrencies");
        }

        private async Task UpdateAccount(AccountDto dbAccount, IList<Services.Accounts.Models.Account> coinbaseProAccounts, IList<ExchangeRateDto> exchangeRates)
        {

            var correspondingCoinbaseProAccount =
                coinbaseProAccounts.FirstOrDefault(x => x.Currency.ToString() == dbAccount.Name);

            if (correspondingCoinbaseProAccount == null)
            {
                _logger.LogInformation($"Couldn't get account {dbAccount.Name} from Coinbase Pro API. Skipping.");
                return;
            }

            var exchangeRateInNok = exchangeRates.FirstOrDefault(x => x.Currency == dbAccount.Name)?.NOKRate;

            if (exchangeRateInNok == null)
            {
                exchangeRateInNok = await GetExchangeRateInNok(dbAccount.Name);
            }

            var valueInNok = (int) (correspondingCoinbaseProAccount.Balance * exchangeRateInNok);

            dbAccount.Balance = valueInNok;

            _dbRepository.QueueUpdate<Account, AccountDto>(dbAccount);
        }

        private async Task<IList<ExchangeRateDto>> GetExchangeRates()
        {
            Response<IList<ExchangeRateDto>> exchangeRates;
            try
            {
                exchangeRates = await _coinbaseApiConnector.GetExchangeRates();
            }
            catch (Exception e)
            {
                throw new CoinbaseApiConnectorException(
                    "Error occured when getting exchange rates from Coinbase API", e);
            }
            
            if (exchangeRates.StatusCode != HttpStatusCode.OK)
            {
                throw new CoinbaseApiConnectorException(
                    $"Error occured when getting exchange rates from Coinbase API. Status code: {exchangeRates.StatusCode}");
            }

            if (!exchangeRates.Success)
            {
                throw new CoinbaseApiConnectorException(
                    $"Error occured when getting exchange rates from Coinbase API. Error message: {exchangeRates.ErrorMessage}");
            }
            
            if (exchangeRates.Data == null)
            {
                throw new CoinbaseApiConnectorException(
                    "Error occured when getting exchange rates from Coinbase API. exchangeRates?.Data?.Rates was null");
            }
            
            return exchangeRates.Data;
        }
        
        private async Task<decimal> GetExchangeRateInNok(string currency)
        {
            Response<ExchangeRateDto> exchangeRates;
            try
            {
                exchangeRates = await _coinbaseApiConnector.GetExchangeRate(currency);
            }
            catch (Exception e)
            {
                throw new CoinbaseApiConnectorException(
                    $"Error occured when getting exchange rates for {currency} from Coinbase API", e);
            }
            
            if (!exchangeRates.Success)
            {
                throw new CoinbaseApiConnectorException(
                    $"Error occured when getting exchange rates for {currency} from Coinbase API. Error message: {exchangeRates.ErrorMessage}");
            }
            
            if (exchangeRates.StatusCode != HttpStatusCode.OK)
            {
                throw new CoinbaseApiConnectorException(
                    $"Error occured when getting exchange rates for {currency} from Coinbase API. Status code: {exchangeRates.StatusCode}");
            }
            
            if (exchangeRates.Data?.NOKRate == null)
            {
                throw new CoinbaseApiConnectorException(
                    $"Error occured when getting exchange rates for {currency} from Coinbase API. exchangeRates?.Data?.Rates was null");
            }

            return exchangeRates.Data.NOKRate;
        }
    }
}