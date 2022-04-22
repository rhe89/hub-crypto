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

public interface IExchangeRateProvider
{
    Task<IList<ExchangeRateDto>> GetExchangeRates(ExchangeRateSearchParameters exchangeRateSearchParameters);
}
    
public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IHubDbRepository _dbRepository;

    public ExchangeRateProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
        
    public async Task<IList<ExchangeRateDto>> GetExchangeRates(ExchangeRateSearchParameters exchangeRateSearchParameters)
    {
        Expression<Func<ExchangeRate, bool>> predicate = exchangeRate =>
            exchangeRateSearchParameters.Currencies == null || exchangeRateSearchParameters.Currencies.Any(currency => exchangeRate.Currency == currency);
                
        return await _dbRepository.WhereAsync<ExchangeRate, ExchangeRateDto>(predicate);
    }
}