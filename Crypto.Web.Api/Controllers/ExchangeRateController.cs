using System.Threading.Tasks;
using Crypto.Providers;
using Hub.Shared.DataContracts.Crypto.SearchParameters;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRateProvider _exchangeRatesProvider;

    public ExchangeRatesController(IExchangeRateProvider exchangeRatesProvider)
    {
        _exchangeRatesProvider = exchangeRatesProvider;
    }
        
    [HttpPost]
    public async Task<IActionResult> ExchangeRates(ExchangeRateSearchParameters exchangeRateSearchParameters)
    {
        var exchangeRates = await _exchangeRatesProvider.GetExchangeRates(exchangeRateSearchParameters);

        return Ok(exchangeRates);
    }
}