using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace CoinbasePro.Data.Entities
{
    [UsedImplicitly]
    [Table("AccountBalance")]
    public class AccountBalance : EntityBase
    {
        [UsedImplicitly]
        [Column]
        public long AccountId { get; set; }
        
        [UsedImplicitly]
        [Column]
        public int Value { get; set; }

        [UsedImplicitly]
        public virtual Account Account { get; set; }  
    }
}