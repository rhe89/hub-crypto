using System.Collections.Generic;
using Hub.Storage.Repository.Entities;

namespace CoinbasePro.Core.Entities
{
    public class Account : EntityBase
    {
        public string Currency { get; set; }
        public decimal CurrentBalance { get; set; }

        public ICollection<AccountBalance> Assets { get; set; }
    }
}
