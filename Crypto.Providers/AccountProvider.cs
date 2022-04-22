﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto;
using Hub.Shared.Storage.Repository.Core;

namespace Crypto.Providers;

public interface IAccountProvider
{
    Task<IList<AccountDto>> GetAccounts(string accountName);
}
    
public class AccountProvider : IAccountProvider
{
    private readonly IHubDbRepository _dbRepository;

    public AccountProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<IList<AccountDto>> GetAccounts(string accountName)
    {
        Expression<Func<Account, bool>> predicate = account =>
            string.IsNullOrEmpty(accountName) || account.Currency.ToLower().Contains(accountName.ToLower());
                
        var accounts = await _dbRepository
            .WhereAsync<Account, AccountDto>(predicate);

        return accounts;
    }
}