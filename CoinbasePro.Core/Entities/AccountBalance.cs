using Hub.Storage.Repository.Entities;

namespace CoinbasePro.Core.Entities
{
    public class AccountBalance : EntityBase
    {
        public long AccountId { get; set; }
        public int Value { get; set; }

        public virtual Account Account { get; set; }  
    }
}