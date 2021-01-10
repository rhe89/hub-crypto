using System.Collections.Generic;

namespace CoinbasePro.Core.Dto.Integration
{
    public class ExchangeRatesDto
    {
        public string Currency { get; set; }
        public IDictionary<string, decimal> Rates { get; set; }
    }
}