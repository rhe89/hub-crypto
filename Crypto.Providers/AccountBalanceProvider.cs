using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.Query;
using Hub.Shared.Storage.Repository.Core;

namespace Crypto.Providers;

public interface IAccountBalanceProvider
{
    Task<IList<AccountBalanceDto>> Get();
    Task<IList<AccountBalanceDto>> Get(AccountQuery query);
}
    
public class AccountBalanceProvider : IAccountBalanceProvider
{
    private readonly IHubDbRepository _dbRepository;

    public AccountBalanceProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<IList<AccountBalanceDto>> Get()
    {
        return await Get(new AccountQuery());
    }

    public async Task<IList<AccountBalanceDto>> Get(AccountQuery query)
    {
        var entities = await _dbRepository
            .GetAsync<AccountBalance, AccountBalanceDto>(new Queryable<AccountBalance>
            {
                Where = entity => 
                    (query.Id == null || query.Id == entity.Id) && 
                    (query.AccountId == null || query.AccountId == entity.AccountId) && 
                    (query.Currency == null || query.Currency == entity.Account.Currency.Name) && 
                    (query.WalletName == null || query.WalletName == entity.Account.Wallet.Name) && 
                    (query.BalanceFromDate == null || entity.BalanceDate.Date >= query.BalanceFromDate.Value.Date) &&
                    (query.BalanceToDate == null || entity.BalanceDate.Date <= query.BalanceToDate.Value.Date),
                Includes = new List<Expression<Func<AccountBalance, object>>>
                {
                    entity => entity.Account,
                    entity => entity.Account.Wallet,
                    entity => entity.Account.Currency
                }
            });

        return entities;
    }
}