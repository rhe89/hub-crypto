using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.SearchParameters;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Providers;

public interface IAssetHistoryProvider
{
    Task<IList<AssetHistoryDto>> GetAssetHistory(AssetHistorySearchParameters assetHistorySearchParameters);
}
    
public class AssetHistoryProvider : IAssetHistoryProvider
{
    private readonly IHubDbRepository _dbRepository;

    public AssetHistoryProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<IList<AssetHistoryDto>> GetAssetHistory(AssetHistorySearchParameters assetHistorySearchParameters)
    {
        Expression<Func<Asset, bool>> predicate = accountBalance =>
            (assetHistorySearchParameters.Currencies == null ||
             assetHistorySearchParameters.Currencies.Any(currency =>
                 accountBalance.Account.Currency.Contains(currency))) &&
            (assetHistorySearchParameters.AccountIds == null ||
             assetHistorySearchParameters.AccountIds.Any(accountId => accountBalance.AccountId == accountId)) &&
            (assetHistorySearchParameters.Exchanges == null ||
             assetHistorySearchParameters.Exchanges.Any(exchange =>
                 accountBalance.Account.Exchange.Contains(exchange))) &&
            (assetHistorySearchParameters.FromDate == null ||
             accountBalance.CreatedDate >= assetHistorySearchParameters.FromDate.Value) &&
            (assetHistorySearchParameters.ToDate == null ||
             accountBalance.CreatedDate <= assetHistorySearchParameters.ToDate.Value);

        var assets = await _dbRepository
            .WhereAsync<Asset, AssetHistoryDto>(predicate, queryable => queryable.Include(x => x.Account));

        return assets;
    }
}