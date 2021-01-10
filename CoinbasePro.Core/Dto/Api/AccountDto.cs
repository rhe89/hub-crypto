namespace CoinbasePro.Core.Dto.Api
{
    public class AccountDto
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal LastMonthBalance { get; set; }
        public decimal LastYearBalance { get; set; }
    }
}
