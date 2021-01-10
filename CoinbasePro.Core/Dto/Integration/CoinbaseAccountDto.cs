using System;

namespace CoinbasePro.Core.Dto.Integration
{
    public class CoinbaseAccount
    {
        public string Currency { get; set; }
        public decimal Assets { get; set; }
        public decimal NativeBalance { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}