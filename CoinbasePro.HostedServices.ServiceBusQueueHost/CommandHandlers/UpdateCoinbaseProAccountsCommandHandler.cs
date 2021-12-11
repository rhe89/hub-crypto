using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinbasePro.Data.Entities;
using CoinbasePro.Integration;
using Hub.Shared.DataContracts.Banking;
using Hub.Shared.DataContracts.Coinbase;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers;

public interface IUpdateCoinbaseProAccountsCommandHandler
{
    Task UpdateAccountAssets();
}
    
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
            _logger.LogInformation("Updating account {Number} of {AccountsCount}: {AccountName}", counter++, accountsCount,dbAccount.Name);

            try
            {
                await UpdateAccount(dbAccount, coinbaseProAccounts, exchangeRates);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed updating account {AccountName}", dbAccount.Name);
            }
        }

        await _dbRepository.ExecuteQueueAsync();

        _logger.LogInformation("Done updating cryptocurrencies");
    }

    private async Task UpdateAccount(AccountDto dbAccount, IEnumerable<Services.Accounts.Models.Account> coinbaseProAccounts, IEnumerable<ExchangeRateDto> exchangeRates)
    {
        var correspondingCoinbaseProAccount =
            coinbaseProAccounts.FirstOrDefault(x => x.Currency.ToString() == dbAccount.Name);

        if (correspondingCoinbaseProAccount == null)
        {
            _logger.LogWarning("Couldn't get account {Account} from Coinbase Pro API", dbAccount.Name);
            return;
        }

        var exchangeRateInNok = exchangeRates.FirstOrDefault(x => x.Currency == dbAccount.Name)?.NOKRate ?? 
                                await GetExchangeRateInNok(dbAccount.Name);

        var valueInNok = (int) (correspondingCoinbaseProAccount.Balance * exchangeRateInNok);

        dbAccount.Balance = valueInNok;

        _dbRepository.QueueUpdate<Account, AccountDto>(dbAccount);
    }

    private async Task<IList<ExchangeRateDto>> GetExchangeRates()
    {
        var exchangeRates = await _coinbaseApiConnector.GetExchangeRates();

        return exchangeRates;
    }
        
    private async Task<decimal> GetExchangeRateInNok(string currency)
    {
        var exchangeRates = await _coinbaseApiConnector.GetExchangeRate(currency);
           
        return exchangeRates.NOKRate;
    }
}