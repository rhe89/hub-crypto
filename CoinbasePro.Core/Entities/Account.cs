using System.Collections.Generic;
using Hub.Storage.Repository.Entities;

namespace CoinbasePro.Core.Entities
{
    public class Account : EntityBase
    {
        public string Currency { get; set; }

        public ICollection<Asset> Assets { get; set; }
    }
}
