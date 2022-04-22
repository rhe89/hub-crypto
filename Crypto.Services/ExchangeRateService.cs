using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Crypto.Integration;
using Crypto.Providers;
using Crypto.Shared.Constants;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.SearchParameters;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace Crypto.Services;

public interface IExchangeRateService
{
    Task<ExchangeRateDto?> GetExchangeRate(string currency);
    Task UpdateExchangeRate(ExchangeRateDto exchangeRateInDb);
    Task<ExchangeRateDto?> CreateOrUpdateExchangeRate(string currency);
}

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly ICoinbaseConnector _coinbaseConnector;
    private readonly IHubDbRepository _dbRepository;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(
        IExchangeRateProvider exchangeRateProvider, 
        ICoinbaseConnector coinbaseConnector, 
        IHubDbRepository dbRepository,
        ILogger<ExchangeRateService> logger)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _coinbaseConnector = coinbaseConnector;
        _dbRepository = dbRepository;
        _logger = logger;
    }

    public async Task<ExchangeRateDto?> GetExchangeRate(string currency)
    {
        var exchangeRates = await _exchangeRateProvider.GetExchangeRates(new ExchangeRateSearchParameters { Currencies = new [] { currency} } );

        return exchangeRates.FirstOrDefault() ?? await CreateOrUpdateExchangeRate(currency);
    }
    
    public async Task<ExchangeRateDto?> CreateOrUpdateExchangeRate(string currency)
    {
        var exchangeRateFromCoinbase =
            await _coinbaseConnector.GetExchangeRatesForCurrency(currency);

        if (exchangeRateFromCoinbase == null)
        {
            _logger.LogError("Data from Coinbase for exchange rate {Currency} was null", currency);
            return null;
        }

        var nokRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.NOK];
        var usdRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.USD];
        var eurRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.EUR];

        var exchangeRates = await _exchangeRateProvider.GetExchangeRates(new ExchangeRateSearchParameters { Currencies = new [] { currency} } );

        var exchangeRate = exchangeRates.FirstOrDefault();

        if (exchangeRate != null)
        {
            exchangeRate.NOKRate = nokRate;
            exchangeRate.USDRate = usdRate;
            exchangeRate.EURRate = eurRate;

            _dbRepository.QueueUpdate<ExchangeRate, ExchangeRateDto>(exchangeRate);
        }
        else
        {
            exchangeRate = new ExchangeRateDto
            {
                Currency = currency,
                NOKRate = nokRate,
                USDRate = usdRate,
                EURRate = eurRate
            };
            
            exchangeRate = _dbRepository.Add<ExchangeRate, ExchangeRateDto>(exchangeRate);
        }

        await _dbRepository.ExecuteQueueAsync();

        return exchangeRate;
    }
    
    public async Task UpdateExchangeRate(ExchangeRateDto exchangeRateInDb)
    {
        var exchangeRateFromCoinbase =
            await _coinbaseConnector.GetExchangeRatesForCurrency(exchangeRateInDb.Currency);

        if (exchangeRateFromCoinbase == null)
        {
            _logger.LogError("Data from Coinbase for exchange rate {Currency} was null", exchangeRateInDb.Currency);
            return;
        }

        var nokRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.NOK];
        var usdRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.USD];
        var eurRate = exchangeRateFromCoinbase.Rates[ExchangeRateConstants.EUR];

        exchangeRateInDb.NOKRate = nokRate;
        exchangeRateInDb.USDRate = usdRate;
        exchangeRateInDb.EURRate = eurRate;

        _dbRepository.QueueUpdate<ExchangeRate, ExchangeRateDto>(exchangeRateInDb);
    }
}