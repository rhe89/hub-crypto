using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Crypto.Integration;
using Crypto.Services;
using Hub.Shared.DataContracts.Crypto.Dto;
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
        
        var coinbaseProAccountsCount = coinbaseProAccounts.Count;

        var counter = 0;
        
        _logger.LogInformation("Updating {Count} CoinbasePro-accounts", coinbaseProAccountsCount);

        var timer = new Stopwatch();
        
        timer.Start();
        
        foreach (var coinbaseProAccount in coinbaseProAccounts)
        {
            try
            {
                await UpdateAccount(coinbaseProAccount, accountsInDb);
                
                counter++;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed updating CoinbasePro-account {AccountName}", coinbaseProAccount.Currency);
            }
        }

        await _dbRepository.ExecuteQueueAsync();
        
        timer.Stop();

        _logger.LogInformation("Done updating {Counter} of {Total} CoinbasePro-accounts in {Seconds} seconds", counter, coinbaseProAccountsCount, timer.Elapsed.TotalSeconds);
    }

    private async Task UpdateAccount(CoinbasePro.Services.Accounts.Models.Account coinbaseProAccount, IEnumerable<AccountDto> accountsInDb)
    {
        var exchangeRate = await _exchangeRateService.GetExchangeRate(coinbaseProAccount.Currency);
        
        if (exchangeRate == null)
        {
            _logger.LogWarning("Could not get exchange rate for currency {Currency}", coinbaseProAccount.Currency);
            return;
        }
        
        var correspondingAccountInDb =
            accountsInDb.FirstOrDefault(x => x.Currency.ToString() == coinbaseProAccount.Currency);

        if (correspondingAccountInDb == null)
        {
            correspondingAccountInDb = new AccountDto
            {
                Currency = coinbaseProAccount.Currency,
                Balance = coinbaseProAccount.Balance * exchangeRate.NOKRate,
                Exchange = "CoinbasePro"
            };
            
            _dbRepository.QueueAdd<Account, AccountDto>(correspondingAccountInDb);
        }
        else
        {
            correspondingAccountInDb.Balance = coinbaseProAccount.Balance * exchangeRate.NOKRate;
            _dbRepository.QueueUpdate<Account, AccountDto>(correspondingAccountInDb);
        }
    }
}