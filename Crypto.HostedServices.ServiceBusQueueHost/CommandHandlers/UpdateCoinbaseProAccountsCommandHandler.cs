using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Crypto.Integration;
using Crypto.Services;
using Hub.Shared.DataContracts.Crypto;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;

public interface IUpdateCoinbaseProAccountsCommandHandler
{
    Task UpdateAccountAssets();
}
    
public class UpdateCoinbaseProAccountsCommandHandler : IUpdateCoinbaseProAccountsCommandHandler
{
    private readonly ILogger<UpdateCoinbaseProAccountsCommandHandler> _logger;
    private readonly ICoinbaseProConnector _coinbaseProConnector;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IHubDbRepository _dbRepository;

    public UpdateCoinbaseProAccountsCommandHandler(ILogger<UpdateCoinbaseProAccountsCommandHandler> logger,
        ICoinbaseProConnector coinbaseProConnector,
        IExchangeRateService exchangeRateService,
        IHubDbRepository dbRepository)
    {
        _logger = logger;
        _coinbaseProConnector = coinbaseProConnector;
        _exchangeRateService = exchangeRateService;
        _dbRepository = dbRepository;
    }
        
    public async Task UpdateAccountAssets()
    {
        var accountsInDb = await _dbRepository
            .WhereAsync<Account, AccountDto>(x => x.Exchange == "CoinbasePro");
            
        var coinbaseProAccounts = await _coinbaseProConnector.GetAccounts();
        
        var accountsCount = accountsInDb.Count;

        var counter = 1;

        foreach (var dbAccount in accountsInDb)
        {
            _logger.LogInformation("Updating CoinbasePro-account {Number} of {AccountsCount}: {AccountName}", counter++, accountsCount,dbAccount.Name);

            try
            {
                await UpdateAccount(dbAccount, coinbaseProAccounts);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed updating CoinbasePro-account {AccountName}", dbAccount.Name);
            }
        }

        await _dbRepository.ExecuteQueueAsync();

        _logger.LogInformation("Done updating CoinbasePro-accounts");
    }

    private async Task UpdateAccount(AccountDto dbAccount, IEnumerable<CoinbasePro.Services.Accounts.Models.Account> coinbaseProAccounts)
    {
        var correspondingCoinbaseProAccount =
            coinbaseProAccounts.FirstOrDefault(x => x.Currency.ToString() == dbAccount.Name);

        if (correspondingCoinbaseProAccount == null)
        {
            _logger.LogWarning("Couldn't get account {Account} from Coinbase Pro", dbAccount.Name);
            return;
        }

        var exchangeRate = await _exchangeRateService.GetExchangeRate(dbAccount.Name);
        
        if (exchangeRate == null)
        {
            return;
        }

        var valueInNok = (int)correspondingCoinbaseProAccount.Balance * exchangeRate.NOKRate;

        dbAccount.Balance = valueInNok;

        _dbRepository.QueueUpdate<Account, AccountDto>(dbAccount);
    }
}