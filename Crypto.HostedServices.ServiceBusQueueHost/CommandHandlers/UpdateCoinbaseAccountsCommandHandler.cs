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

public interface IUpdateCoinbaseAccountsCommandHandler
{
    Task UpdateAccounts();
}
    
public class UpdateCoinbaseAccountsCommandHandler : IUpdateCoinbaseAccountsCommandHandler
{
    private readonly ILogger<UpdateCoinbaseAccountsCommandHandler> _logger;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ICoinbaseConnector _coinbaseConnector;
    private readonly IHubDbRepository _dbRepository;

    public UpdateCoinbaseAccountsCommandHandler(ILogger<UpdateCoinbaseAccountsCommandHandler> logger,
        IExchangeRateService exchangeRateService,
        IHubDbRepository dbRepository, ICoinbaseConnector coinbaseConnector)
    {
        _logger = logger;
        _exchangeRateService = exchangeRateService;
        _dbRepository = dbRepository;
        _coinbaseConnector = coinbaseConnector;
    }
        
    public async Task UpdateAccounts()
    {
        var accountsInDb = await _dbRepository
            .WhereAsync<Account, AccountDto>(x => x.Exchange == "Coinbase");
            
        var coinbaseAccounts = await _coinbaseConnector.GetAccounts();
            
        var accountsCount = accountsInDb.Count;

        var counter = 1;

        foreach (var dbAccount in accountsInDb)
        {
            _logger.LogInformation("Updating Coinbase-account {AccountName} ({Counter} of {Total})", dbAccount.Name, counter++, accountsCount);

            try
            {
                await UpdateAccount(dbAccount, coinbaseAccounts);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed updating Coinbase-account {Account}", dbAccount.Name);
            }
        }

        await _dbRepository.ExecuteQueueAsync();

        _logger.LogInformation("Done updating {Counter} Coinbase-accounts", counter);
    }

    private async Task UpdateAccount(AccountDto dbAccount, IEnumerable<Coinbase.Models.Account> coinbaseAccounts)
    {
        var correspondingCoinbaseAccount =
            coinbaseAccounts.FirstOrDefault(x => x.Currency.Code == dbAccount.Name);

        if (correspondingCoinbaseAccount == null)
        {
            _logger.LogWarning("Couldn't get account {Account} from Coinbase", dbAccount.Name);
            return;
        }

        var exchangeRate = await _exchangeRateService.GetExchangeRate(dbAccount.Name);

        if (exchangeRate == null)
        {
            return;
        }

        var balance = (int)correspondingCoinbaseAccount.Balance.Amount * exchangeRate.NOKRate;

        dbAccount.Balance = balance;

        _dbRepository.QueueUpdate<Account, AccountDto>(dbAccount);
    }
}