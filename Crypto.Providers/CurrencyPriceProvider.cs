using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.Query;
using Hub.Shared.Storage.Repository.Core;

namespace Crypto.Providers;

public interface ICurrencyPriceProvider
{
    Task<IList<CurrencyPriceDto>> Get();
    Task<IList<CurrencyPriceDto>> Get(CurrencyQuery query);
}
    
public class CurrencyPriceProvider : ICurrencyPriceProvider
{
    private readonly IHubDbRepository _dbRepository;

    public CurrencyPriceProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<IList<CurrencyPriceDto>> Get()
    {
        return await Get(new CurrencyQuery());
    }

    public async Task<IList<CurrencyPriceDto>> Get(CurrencyQuery query)
    {
        var entities = await _dbRepository
            .GetAsync<CurrencyPrice, CurrencyPriceDto>(new Queryable<CurrencyPrice>
            {
                Where = entity => 
                    (query.Id == null || query.Id == entity.Id) && 
                    (query.CurrencyId == null || query.CurrencyId == entity.CurrencyId) && 
                    (query.PriceFromDate == null || entity.PriceDate.Date >= query.PriceFromDate.Value.Date) &&
                    (query.PriceToDate == null || entity.PriceDate.Date <= query.PriceToDate.Value.Date),
                Includes = new List<Expression<Func<CurrencyPrice, object>>>
                {
                    entity => entity.Currency,
                }
            });

        return entities;
    }
}