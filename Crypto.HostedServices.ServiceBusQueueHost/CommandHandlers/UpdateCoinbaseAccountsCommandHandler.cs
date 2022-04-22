using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Crypto.Integration;
using Crypto.Services;
using Hub.Shared.DataContracts.Crypto;
using Hub.Shared.DataContracts.Crypto.Dto;
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
            
        var coinbaseAccountsCount = coinbaseAccounts.Count;

        var counter = 1;

        foreach (var coinbaseAccount in coinbaseAccounts)
        {
            _logger.LogInformation("Updating Coinbase-account {AccountName} ({Counter} of {Total})", coinbaseAccount.Currency, counter++, coinbaseAccountsCount);

            try
            {
                await UpdateAccount(coinbaseAccount, accountsInDb);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed updating Coinbase-account {Account}", coinbaseAccount.Currency);
            }
        }

        await _dbRepository.ExecuteQueueAsync();

        _logger.LogInformation("Done updating {Counter} Coinbase-accounts", counter);
    }

    private async Task UpdateAccount(Coinbase.Models.Account coinbaseAccount, IEnumerable<AccountDto> accountsInDb)
    {
        var exchangeRate = await _exchangeRateService.GetExchangeRate(coinbaseAccount.Currency.Code);
        
        if (exchangeRate == null)
        {
            _logger.LogWarning("Could not get exchange rate for currency {Currency}", coinbaseAccount.Currency);
            return;
        }
        
        var correspondingAccountInDb =
            accountsInDb.FirstOrDefault(x => x.Currency.ToString() == coinbaseAccount.Currency.Code);

        if (correspondingAccountInDb == null)
        {
            correspondingAccountInDb = new AccountDto
            {
                Currency = coinbaseAccount.Currency.Code,
                Balance = coinbaseAccount.Balance.Amount * exchangeRate.NOKRate,
                Exchange = "Coinbase"
            };
            
            _dbRepository.QueueAdd<Account, AccountDto>(correspondingAccountInDb);
        }
        else
        {
            correspondingAccountInDb.Balance = coinbaseAccount.Balance.Amount * exchangeRate.NOKRate;
            _dbRepository.QueueUpdate<Account, AccountDto>(correspondingAccountInDb);
        }
    }
}