using System;
using System.Linq;
using System.Threading.Tasks;
using CoinbasePro.Data.Entities;
using Hub.Shared.DataContracts.Banking;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers;

public interface IUpdateCoinbaseProAccountBalanceHistoryCommandHandler
{
    Task UpdateAccountBalance();
}
    
public class UpdateCoinbaseProAccountBalanceHistoryCommandHandler : IUpdateCoinbaseProAccountBalanceHistoryCommandHandler
{
    private readonly IHubDbRepository _dbRepository;
    private readonly ILogger<UpdateCoinbaseProAccountBalanceHistoryCommandHandler> _logger;

    public UpdateCoinbaseProAccountBalanceHistoryCommandHandler(ILogger<UpdateCoinbaseProAccountBalanceHistoryCommandHandler> logger,
        IHubDbRepository dbRepository)
    {
        _logger = logger;
        _dbRepository = dbRepository;
    }
        
    public async Task UpdateAccountBalance()
    {
        var accounts = await _dbRepository.AllAsync<Account, AccountDto>();
            
        foreach (var account in accounts)
        {
            var now = DateTime.Now;

            _logger.LogInformation("Updating account balance history for account {AccountName}", account.Name);
                
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
            
        _logger.LogInformation("Finished updating account balance history");

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