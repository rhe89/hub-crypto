using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CoinbasePro.Core.Dto.Data;
using CoinbasePro.Core.Dto.Integration;
using CoinbasePro.Core.Entities;
using CoinbasePro.Core.Exceptions;
using CoinbasePro.Core.Integration;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Factories;
using Hub.Storage.Core.Providers;
using Hub.Storage.Core.Repository;
using Hub.Web.Http;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.BackgroundTasks
{
    public class UpdateAccountsTask : BackgroundTask
    {
        private readonly ILogger<UpdateAccountsTask> _logger;
        private readonly ICoinbaseProConnector _coinbaseProConnector;
        private readonly ICoinbaseApiConnector _coinbaseApiConnector;
        private readonly IHubDbRepository _dbRepository;

        public UpdateAccountsTask(IBackgroundTaskConfigurationProvider backgroundTaskConfigurationProvider,
            IBackgroundTaskConfigurationFactory backgroundTaskConfigurationFactory,
            ILogger<UpdateAccountsTask> logger,
            ICoinbaseProConnector coinbaseProConnector,
            ICoinbaseApiConnector coinbaseApiConnector,
            IHubDbRepository dbRepository) : base(backgroundTaskConfigurationProvider, backgroundTaskConfigurationFactory)
        {
            _logger = logger;
            _coinbaseProConnector = coinbaseProConnector;
            _coinbaseApiConnector = coinbaseApiConnector;
            _dbRepository = dbRepository;
        }

        public override async Task Execute(CancellationToken cancellationToken)
        {
            _dbRepository.ToggleDispose(false);

            try
            {
                await UpdateAccountAssets();

                _dbRepository.ToggleDispose(true);

                await _dbRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                _dbRepository.ToggleDispose(true);

                throw;
            }
        }

        private async Task UpdateAccountAssets()
        {
            var accountsInDb = await _dbRepository.AllAsync<Account, AccountDto>();

            var assets = await _dbRepository.AllAsync<Asset, AssetDto>();

            var coinbaseProAccounts = await _coinbaseProConnector.GetAccounts();

            var exchangeRates = await GetExchangeRates();

            var accountsCount = accountsInDb.Count;

            var counter = 1;

            foreach (var dbAccount in accountsInDb)
            {
                _logger.LogInformation($"Updating account {counter++} of {accountsCount}: {dbAccount.Currency}.");

                var correspondingCoinbaseProAccount =
                    coinbaseProAccounts.FirstOrDefault(x => x.Currency.ToString() == dbAccount.Currency);

                if (correspondingCoinbaseProAccount == null)
                {
                    _logger.LogInformation($"Couldn't get account {dbAccount.Currency} from Coinbase Pro API. Skipping.");
                    continue;
                }

                var existingAsset = assets.FirstOrDefault(x =>
                    x.CreatedDate.Date == DateTime.Now.Date &&
                    x.AccountId == dbAccount.Id);

                var exchangeRateInNok = exchangeRates.FirstOrDefault(x => x.Currency == dbAccount.Currency)?.NOKRate;

                if (exchangeRateInNok == null)
                {
                    exchangeRateInNok = await GetExchangeRateInNok(dbAccount.Currency);
                }
                
                var valueInNok = (int) (correspondingCoinbaseProAccount.Balance * exchangeRateInNok);
                
                if (existingAsset != null)
                {
                    _logger.LogInformation($"Updating assets for {dbAccount.Currency}");

                    existingAsset.Value = valueInNok;

                    _dbRepository.Update<Asset, AssetDto>(existingAsset);
                }
                else
                {
                    _logger.LogInformation($"Adding assets for currency {dbAccount.Currency}");

                    var asset = new AssetDto
                    {
                        AccountId = dbAccount.Id,
                        Value = valueInNok
                    };

                    _dbRepository.Add<Asset, AssetDto>(asset);
                }
            }

            _logger.LogInformation($"Done updating cryptocurrencies.");
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
                    $"Error occured when getting exchange rates from Coinbase API", e);
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
                    $"Error occured when getting exchange rates from Coinbase API. exchangeRates?.Data?.Rates was null");
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