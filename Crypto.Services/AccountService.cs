using System;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Crypto.Providers;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.Query;
using Hub.Shared.Storage.Repository.Core;
using Hub.Shared.Utilities;
using Microsoft.Extensions.Logging;

namespace Crypto.Services;

public interface IAccountService
{
    Task<AccountDto> Add(AccountDto newAccount);
    Task<bool> Update(AccountDto updatedAccount, bool saveChanges);
    Task SaveChanges();
}

public class AccountService : IAccountService
{
    private readonly IAccountProvider _accountProvider;
    private readonly IAccountBalanceProvider _accountBalanceProvider;
    private readonly IHubDbRepository _dbRepository;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        IAccountProvider accountProvider,
        IAccountBalanceProvider accountBalanceProvider,
        IHubDbRepository dbRepository,
        ILogger<AccountService> logger)
    {
        _accountProvider = accountProvider;
        _accountBalanceProvider = accountBalanceProvider;
        _dbRepository = dbRepository;
        _logger = logger;
    }
    public async Task<AccountDto> Add(AccountDto newAccount)
    {
        _logger.LogInformation("Creating account");

        var addedAccount = await _dbRepository.AddAsync<Account, AccountDto>(newAccount);

        if (newAccount.BalanceDate != null)
        {
            await UpdateAccountBalance(addedAccount.Id, newAccount.BalanceDate.Value, newAccount.Balance, true);
        }
        
        return addedAccount;
    }

    public async Task<bool> Update(AccountDto updatedAccount, bool saveChanges)
    {
        _logger.LogInformation("Updating account {Name} (Id: {Id})", updatedAccount.Currency.Name, updatedAccount.Id);

        var accountInDb = (await _accountProvider.Get(new AccountQuery
        {
            Id = updatedAccount.Id,
            BalanceToDate = DateTimeUtils.Today
        })).First();

        accountInDb.Name = updatedAccount.Name;
        accountInDb.WalletId = updatedAccount.WalletId;
        accountInDb.CurrencyId = updatedAccount.CurrencyId;

        if (updatedAccount.BalanceDate != null)
        {
            await UpdateAccountBalance(accountInDb.Id, updatedAccount.BalanceDate.Value, updatedAccount.Balance, saveChanges);
        }

        _dbRepository.QueueUpdate<Account, AccountDto>(accountInDb);

        if (saveChanges)
        {
            await _dbRepository.ExecuteQueueAsync();
        }
        
        return true;
    }
    
    private async Task UpdateAccountBalance(long accountId, DateTime balanceDate, decimal balance, bool saveChanges)
    {
        var accountBalance = (await _accountBalanceProvider.Get(new AccountQuery
        {
            AccountId = accountId,
            BalanceFromDate = balanceDate,
            BalanceToDate = balanceDate,
        })).FirstOrDefault();

        if (accountBalance == null)
        {
            accountBalance = new AccountBalanceDto
            {
                AccountId = accountId,
                BalanceDate = balanceDate,
                Balance = balance
            };
            
            if (saveChanges)
            {
                await _dbRepository.AddAsync<AccountBalance, AccountBalanceDto>(accountBalance);
            }
            else
            {
                _dbRepository.QueueAdd<AccountBalance, AccountBalanceDto>(accountBalance);
            }
        }
        else
        {
            accountBalance.Balance = balance;
            
            if (saveChanges)
            {
                await _dbRepository.UpdateAsync<AccountBalance, AccountBalanceDto>(accountBalance);
            }
            else
            {
                _dbRepository.QueueUpdate<AccountBalance, AccountBalanceDto>(accountBalance);
            }
        }
    }

    public async Task SaveChanges()
    {
        await _dbRepository.ExecuteQueueAsync();
    }
    
}