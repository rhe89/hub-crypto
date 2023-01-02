using System.Collections.Generic;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.Query;
using Hub.Shared.Storage.Repository.Core;

namespace Crypto.Providers;

public interface ICurrencyProvider
{
    Task<IList<CurrencyDto>> Get();
    Task<IList<CurrencyDto>> Get(CurrencyQuery query);
}
    
public class CurrencyProvider : ICurrencyProvider
{
    private readonly IHubDbRepository _dbRepository;

    public CurrencyProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<IList<CurrencyDto>> Get()
    {
        return await Get(new CurrencyQuery());
    }
    
    public async Task<IList<CurrencyDto>> Get(CurrencyQuery query)
    {
        var entities = await _dbRepository
            .GetAsync<Currency, CurrencyDto>(new Queryable<Currency>
            {
                Where = entity => 
                    (query.Id == null || query.Id == entity.Id) && 
                    (query.Name == null || query.Name == entity.Name)
            });

        return entities;
    }
}