using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace Crypto.Data.Entities;

[UsedImplicitly]
public class Account : EntityBase
{
    [UsedImplicitly]
    [Column]
    public string Name { get; set; }
    
    [UsedImplicitly]
    [Column]
    public long WalletId { get; set; }
    
    [UsedImplicitly]
    [Column]
    public long CurrencyId { get; set; }

    [UsedImplicitly]
    public virtual Wallet Wallet { get; set; }
    
    [UsedImplicitly]
    public virtual Currency Currency { get; set; }
}