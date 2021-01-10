using Hub.Storage.Core.Entities;

namespace CoinbasePro.Core.Entities
{
    public class Asset : EntityBase
    {
        public long AccountId { get; set; }
        public int Value { get; set; }

        public Account Account { get; set; }  
    }
}