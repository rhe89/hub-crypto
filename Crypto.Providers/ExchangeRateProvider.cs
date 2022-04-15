using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto;
using Hub.Shared.Storage.Repository.Core;

namespace Crypto.Providers;

public interface IExchangeRateProvider
{
    Task<ExchangeRateDto> GetExchangeRate(string currency);
}
    
public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IHubDbRepository _dbRepository;

    public ExchangeRateProvider(IHubDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
        
    public async Task<ExchangeRateDto> GetExchangeRate(string currency)
    {
        Expression<Func<ExchangeRate, bool>> predicate = exchangeRate =>
            string.IsNullOrEmpty(currency) || exchangeRate.Currency.ToLower().Contains(currency.ToLower());
                
        return await _dbRepository.FirstOrDefaultAsync<ExchangeRate, ExchangeRateDto>(predicate);
    }
}