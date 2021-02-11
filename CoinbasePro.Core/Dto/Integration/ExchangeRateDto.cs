namespace CoinbasePro.Core.Dto.Integration
{
    public class ExchangeRateDto
    {
        public string Currency { get; set; }
        public decimal NOKRate { get; set; }
        public decimal USDRate { get; set; }
        public decimal EURRate { get; set; }
    }
}