using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.Query;
using Hub.Shared.Storage.Repository.Core;
using Hub.Shared.Utilities;

namespace Crypto.Providers;

public interface IAccountProvider
{
    Task<IList<AccountDto>> Get();
    Task<IList<AccountDto>> Get(AccountQuery query);
}
    
public class AccountProvider : IAccountProvider
{
    private readonly IAccountBalanceProvider _accountBalanceProvider;
    private readonly IHubDbRepository _dbRepository;

    public AccountProvider(
        IAccountBalanceProvider accountBalanceProvider,
        IHubDbRepository dbRepository)
    {
        _accountBalanceProvider = accountBalanceProvider;
        _dbRepository = dbRepository;
    }

    public async Task<IList<AccountDto>> Get()
    {
        return await Get(new AccountQuery());
    }

    public async Task<IList<AccountDto>> Get(AccountQuery query)
    {
        var accounts = await _dbRepository
            .GetAsync<Account, AccountDto>(new Queryable<Account>
            {
                Where = entity => 
                    (query.Id == null || query.Id == entity.Id) && 
                    (query.Currency == null || query.Currency == entity.Currency.Name) && 
                    (query.WalletName == null || query.WalletName == entity.Wallet.Name),
                Includes = new List<Expression<Func<Account, object>>>
                {
                    entity => entity.Currency,
                    entity => entity.Wallet
                }
            });
        
        var accountBalanceQuery = query;

        if (accountBalanceQuery.BalanceFromDate != null ||
            accountBalanceQuery.BalanceToDate != null)
        {
            accountBalanceQuery.AccountId = accountBalanceQuery.Id;
            accountBalanceQuery.Id = null;

            var accountBalances = await _accountBalanceProvider.Get(accountBalanceQuery);

            foreach (var account in accounts)
            {
                var accountBalance = accountBalances.Where(x => x.AccountId == account.Id).MaxBy(x => x.BalanceDate);
                
                if (accountBalance?.BalanceDate != null && accountBalanceQuery.BalanceToDate != null)
                {
                    account.NoBalanceForGivenPeriod =
                        DateTimeUtils.LastDayOfMonth(accountBalanceQuery.BalanceToDate.Value.Year,
                                                     accountBalanceQuery.BalanceToDate.Value.Month) >
                        DateTimeUtils.LastDayOfMonth(accountBalance.BalanceDate.Year, accountBalance.BalanceDate.Month);
                }
                else
                {
                    account.NoBalanceForGivenPeriod = accountBalance?.BalanceDate == null;
                }
                
                account.Balance = accountBalance?.Balance ?? 0;
                account.BalanceDate = accountBalance?.BalanceDate ?? DateTimeUtils.Today;
            }
        }

        return accounts;
    }
}