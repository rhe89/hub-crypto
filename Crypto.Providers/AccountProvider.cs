using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.SearchParameters;
using Hub.Shared.Storage.Repository.Core;

namespace Crypto.Providers;

public interface IAccountProvider
{
    Task<IList<AccountDto>> GetAccounts(AccountSearchParameters accountName);
}
    
public class AccountProvider : IAccountProvider
{
    private readonly IHubDbRepository _dbRepository;

    public AccountProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<IList<AccountDto>> GetAccounts(AccountSearchParameters accountSearchParameters)
    {
        Expression<Func<Account, bool>> predicate = account => 
            (accountSearchParameters.Currencies == null || accountSearchParameters.Currencies.Any(currency => account.Currency.Contains(currency))) && 
            (accountSearchParameters.AccountIds == null || accountSearchParameters.AccountIds.Any(accountId => account.Id == accountId)) && 
            (accountSearchParameters.Exchanges == null || accountSearchParameters.Exchanges.Any(bank => account.Exchange.Contains(bank)));
                
        var accounts = await _dbRepository
            .WhereAsync<Account, AccountDto>(predicate);

        if (!accountSearchParameters.MergeAccountsWithSameNameFromDifferentExchanges)
        {
            return accounts;
        }
        
        var mergedAccounts = new Dictionary<string, AccountDto>();

        foreach (var account in accounts)
        {
            if (mergedAccounts.TryGetValue(account.Currency, out var mergedAccount))
            {
                mergedAccount.Balance += account.Balance;
                mergedAccount.Exchange += $", {account.Exchange}";
                mergedAccount.MergedAccount = true;
            }
            else
            {
                mergedAccounts.Add(account.Currency, account);
            }
        }

        return mergedAccounts.Values.ToList();
    }
}