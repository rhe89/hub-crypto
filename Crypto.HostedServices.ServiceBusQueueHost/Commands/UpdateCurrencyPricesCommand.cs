using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crypto.Integration;
using Crypto.Providers;
using Crypto.Services;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Hub.Shared.Storage.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Crypto.HostedServices.ServiceBusQueueHost.Commands;

public class UpdateCurrencyPricesCommand : ServiceBusQueueCommand
{
    private readonly ICoinMarketCapConnector _coinMarketCapConnector;
    private readonly ICurrencyService _currencyService;
    private readonly ICurrencyProvider _currencyProvider;
    private readonly ILogger<UpdateCurrencyPricesCommand> _logger;

    public UpdateCurrencyPricesCommand(
        ICoinMarketCapConnector coinMarketCapConnector,
        ICurrencyService currencyService,
        ICurrencyProvider currencyProvider,
        ILogger<UpdateCurrencyPricesCommand> logger)
    {
        _coinMarketCapConnector = coinMarketCapConnector;
        _currencyService = currencyService;
        _currencyProvider = currencyProvider;
        _logger = logger;
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var currencies = await _currencyProvider.Get();

        _logger.LogInformation("Got {Count} currencies from database", currencies.Count);
        
        var currenciesFromCoinMarketCap = _coinMarketCapConnector.GetCurrencies(currencies.Select(x => x.Name).ToArray());

        _logger.LogInformation("Got {Count} currencies from CoinMarketCap", currenciesFromCoinMarketCap.Count);

        foreach (var currency in currencies)
        {
            _logger.LogInformation("Updating {Currency}", currency.Name);

            var currencyFromCoinMarketCap = currenciesFromCoinMarketCap.FirstOrDefault(x => x.Symbol == currency.Name);

            if (currencyFromCoinMarketCap == null)
            {
                _logger.LogWarning("No corresponding currency found at CoinMarketCap for {Currency}", currency.Name);
                continue;
            }
            
            await _currencyService.UpdateCurrencyPrice(
                currency.Id,
                currencyFromCoinMarketCap.LastUpdated,
                (decimal)currencyFromCoinMarketCap.Price,
                false);
        }

        await _currencyService.SaveChanges();
    }

    public override string Trigger => QueueNames.UpdateCurrencyPrices;
}