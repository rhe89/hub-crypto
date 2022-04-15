using System;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;

public interface IUpdateCoinbaseAccountBalanceHistoryCommandHandler
{
    Task UpdateAccountBalance();
}
    
public class UpdateCoinbaseAccountBalanceHistoryCommandHandler : IUpdateCoinbaseAccountBalanceHistoryCommandHandler
{
    private readonly IHubDbRepository _dbRepository;
    private readonly ILogger<UpdateCoinbaseAccountBalanceHistoryCommandHandler> _logger;

    public UpdateCoinbaseAccountBalanceHistoryCommandHandler(ILogger<UpdateCoinbaseAccountBalanceHistoryCommandHandler> logger,
        IHubDbRepository dbRepository)
    {
        _logger = logger;
        _dbRepository = dbRepository;
    }
        
    public async Task UpdateAccountBalance()
    {
        var accountsInDb = await _dbRepository
            .WhereAsync<Account, AccountDto>(x => x.Exchange == "Coinbase");
        
        foreach (var account in accountsInDb)
        {
            var now = DateTime.Now;

            _logger.LogInformation("Updating Coinbase-account balance history for account {Account}", account.Name);
                
            var accountBalanceForCurrentDay = GetAccountBalanceForCurrentDay(account, now);
            
            if (accountBalanceForCurrentDay == null)
            {
                AddAccountBalance(account);
            }
            else
            {
                UpdateAccountBalance(accountBalanceForCurrentDay, account);
            }            
        }
            
        await _dbRepository.ExecuteQueueAsync();
            
        _logger.LogInformation("Finished updating Coinbase-account balance history");

    }

    private AccountBalanceDto GetAccountBalanceForCurrentDay(AccountDto account, DateTime now)
    {
        return _dbRepository.All<AccountBalance, AccountBalanceDto>().FirstOrDefault(x =>
            x.AccountId == account.Id &&
            x.CreatedDate.Year == now.Year &&
            x.CreatedDate.Month == now.Month &&
            x.CreatedDate.Day == now.Day);
    }

        
    private void AddAccountBalance(AccountDto account)
    {
        var accountBalanceForCurrentDay = new AccountBalanceDto
        {
            AccountId = account.Id,
            Balance = (int)account.Balance
        };

        _dbRepository.QueueAdd<AccountBalance, AccountBalanceDto>(accountBalanceForCurrentDay);
    }
        
    private void UpdateAccountBalance(AccountBalanceDto accountBalanceForCurrentDay, AccountDto account)
    {
        accountBalanceForCurrentDay.Balance = (int)account.Balance;

        _dbRepository.QueueUpdate<AccountBalance, AccountBalanceDto>(accountBalanceForCurrentDay);
    }

}