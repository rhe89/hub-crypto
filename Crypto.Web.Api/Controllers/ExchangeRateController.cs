using System.Threading.Tasks;
using Crypto.Providers;
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
        
    public async Task<IActionResult> ExchangeRates([FromQuery]string currency)
    {
        var exchangeRate = await _exchangeRatesProvider.GetExchangeRate(currency);

        return Ok(exchangeRate);
    }
}