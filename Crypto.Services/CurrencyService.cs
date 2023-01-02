using System;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Crypto.Providers;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.DataContracts.Crypto.Query;
using Hub.Shared.Storage.Repository.Core;
using Hub.Shared.Utilities;
using Microsoft.Extensions.Logging;

namespace Crypto.Services;

public interface ICurrencyService
{
    Task<CurrencyDto> Add(CurrencyDto newCurrency);
    Task<bool> Update(CurrencyDto updatedCurrency, bool saveChanges);
    Task UpdateCurrencyPrice(long currencyId, DateTime balanceDate, decimal balance, bool saveChanges);
    Task SaveChanges();
}

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyProvider _currencyProvider;
    private readonly ICurrencyPriceProvider _currencyPriceProvider;
    private readonly IHubDbRepository _dbRepository;
    private readonly ILogger<CurrencyService> _logger;

    public CurrencyService(
        ICurrencyProvider currencyProvider,
        ICurrencyPriceProvider currencyPriceProvider,
        IHubDbRepository dbRepository,
        ILogger<CurrencyService> logger)
    {
        _currencyProvider = currencyProvider;
        _currencyPriceProvider = currencyPriceProvider;
        _dbRepository = dbRepository;
        _logger = logger;
    }
    
    public async Task<CurrencyDto> Add(CurrencyDto newCurrency)
    {
        _logger.LogInformation("Creating currency");

        var addedCurrency = await _dbRepository.AddAsync<Currency, CurrencyDto>(newCurrency);

        if (newCurrency.PriceDate != null)
        {
            await UpdateCurrencyPrice(addedCurrency.Id, newCurrency.PriceDate.Value, newCurrency.Price, true);
        }
        
        return addedCurrency;
    }

    public async Task<bool> Update(CurrencyDto updatedCurrency, bool saveChanges)
    {
        _logger.LogInformation("Updating currency {Name} (Id: {Id})", updatedCurrency.Name, updatedCurrency.Id);

        var currencyInDb = (await _currencyProvider.Get(new CurrencyQuery
        {
            Id = updatedCurrency.Id,
            PriceToDate = DateTimeUtils.Today
        })).First();

        currencyInDb.Name = updatedCurrency.Name;

        if (updatedCurrency.PriceDate != null)
        {
            await UpdateCurrencyPrice(currencyInDb.Id, updatedCurrency.PriceDate.Value, updatedCurrency.Price, saveChanges);
        }

        _dbRepository.QueueUpdate<Currency, CurrencyDto>(currencyInDb);

        if (saveChanges)
        {
            await _dbRepository.ExecuteQueueAsync();
        }
        
        return true;
    }

    public async Task UpdateCurrencyPrice(long currencyId, DateTime balanceDate, decimal balance, bool saveChanges)
    {
        var currencyPrice = (await _currencyPriceProvider.Get(new CurrencyQuery
        {
            CurrencyId = currencyId,
            PriceFromDate = balanceDate,
            PriceToDate = balanceDate,
        })).FirstOrDefault();

        if (currencyPrice == null)
        {
            currencyPrice = new CurrencyPriceDto
            {
                CurrencyId = currencyId,
                PriceDate = balanceDate,
                Price = balance
            };
            
            if (saveChanges)
            {
                await _dbRepository.AddAsync<CurrencyPrice, CurrencyPriceDto>(currencyPrice);
            }
            else
            {
                _dbRepository.QueueAdd<CurrencyPrice, CurrencyPriceDto>(currencyPrice);
            }
        }
        else
        {
            currencyPrice.Price = balance;
            
            if (saveChanges)
            {
                await _dbRepository.UpdateAsync<CurrencyPrice, CurrencyPriceDto>(currencyPrice);
            }
            else
            {
                _dbRepository.QueueUpdate<CurrencyPrice, CurrencyPriceDto>(currencyPrice);
            }
        }
    }

    public async Task SaveChanges()
    {
        await _dbRepository.ExecuteQueueAsync();
    }
}