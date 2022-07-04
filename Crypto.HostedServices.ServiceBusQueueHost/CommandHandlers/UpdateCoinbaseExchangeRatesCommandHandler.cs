using System;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Crypto.Services;
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
        
        var exchangeRatesCount = exchangeRatesInDb.Count;

        var counter = 1;
        
        _logger.LogInformation("Updating {Count} exchange rates", exchangeRatesCount);
        
        foreach (var exchangeRateInDb in exchangeRatesInDb)
        {

            try
            {
                await _exchangeRateService.QueueUpdateExchangeRate(exchangeRateInDb);

                counter++;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed updating exchange rate for currency {Currency}", exchangeRateInDb.Currency);
            }
        }

        await _exchangeRateService.FinishUpdateExchangeRates();
            
        _logger.LogInformation("Finished updating {Count} of {exchangeRatesCount} exchange rates", counter, exchangeRatesCount);
    }
}