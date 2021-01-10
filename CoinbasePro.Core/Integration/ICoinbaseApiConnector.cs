using System.Collections.Generic;
using System.Threading.Tasks;
using CoinbasePro.Core.Dto.Integration;
using Hub.Web.Http;

namespace CoinbasePro.Core.Integration
{
    public interface ICoinbaseApiConnector
    {
        Task<Response<ExchangeRatesDto>> GetExchangeRates(string currency);
    }
}