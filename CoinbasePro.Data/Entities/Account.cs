using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace CoinbasePro.Data.Entities;

[UsedImplicitly]
public class Account : EntityBase
{
    [UsedImplicitly]
    [Column]
    public string Currency { get; set; }

    [UsedImplicitly]
    [Column]
    public decimal CurrentBalance { get; set; }

    [UsedImplicitly]
    public ICollection<AccountBalance> Assets { get; set; }
}