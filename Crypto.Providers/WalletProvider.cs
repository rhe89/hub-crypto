using System.Collections.Generic;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.Query;
using Hub.Shared.Storage.Repository.Core;

namespace Crypto.Providers;

public interface IWalletProvider
{
    Task<IList<WalletDto>> Get();
    Task<IList<WalletDto>> Get(WalletQuery query);
}
    
public class WalletProvider : IWalletProvider
{
    private readonly IHubDbRepository _dbRepository;

    public WalletProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<IList<WalletDto>> Get()
    {
        return await Get(new WalletQuery());
    }

    public async Task<IList<WalletDto>> Get(WalletQuery query)
    {
        var entities = await _dbRepository
            .GetAsync<Wallet, WalletDto>(new Queryable<Wallet>
            {
                Where = entity => 
                    (query.Id == null || query.Id == entity.Id) && 
                    (query.Name == null || query.Name == entity.Name)
            });

        return entities;
    }
}