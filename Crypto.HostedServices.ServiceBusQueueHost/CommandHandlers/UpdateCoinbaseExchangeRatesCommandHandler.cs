using System;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Crypto.Services;
using Hub.Shared.DataContracts.Crypto;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;

public interface IUpdateCoinbaseExchangeRatesCommandHandler
{
    Task UpdateExchangeRates();
}
    
public class UpdateCoinbaseExchangeRatesCommandHandler : IUpdateCoinbaseExchangeRatesCommandHandler
{
    private readonly ILogger<UpdateCoinbaseExchangeRatesCommandHandler> _logger;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IHubDbRepository _dbRepository;

    public UpdateCoinbaseExchangeRatesCommandHandler(ILogger<UpdateCoinbaseExchangeRatesCommandHandler> logger,
        IExchangeRateService exchangeRateService,
        IHubDbRepository dbRepository)
    {
        _logger = logger;
        _exchangeRateService = exchangeRateService;
        _dbRepository = dbRepository;
    }

    public async Task UpdateExchangeRates()
    {
        var exchangeRatesInDb = await _dbRepository.AllAsync<ExchangeRate, ExchangeRateDto>();
            
        _logger.LogInformation("Got {Count} exchange rates from database", exchangeRatesInDb.Count);
            
        var exchangeRatesCount = exchangeRatesInDb.Count;

        var counter = 1;
            
        foreach (var exchangeRateInDb in exchangeRatesInDb)
        {
            _logger.LogInformation("Updating {Currency} ({Counter} of {Total})", exchangeRateInDb.Currency, counter++, exchangeRatesCount);

            try
            {
                await _exchangeRateService.UpdateExchangeRate(exchangeRateInDb);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed updating exchange rate for currency {Currency}", exchangeRateInDb.Currency);
            }
        }
            
        await _dbRepository.ExecuteQueueAsync();
            
        _logger.LogInformation("Finished updating {Count} exchange rates", counter);
    }
}