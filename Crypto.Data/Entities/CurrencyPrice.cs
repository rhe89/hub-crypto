using System;
using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace Crypto.Data.Entities;

[UsedImplicitly]
public class CurrencyPrice : EntityBase
{
    [UsedImplicitly]
    [Column]
    public long CurrencyId { get; set; }
    
    [UsedImplicitly]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    [UsedImplicitly]
    [Column]
    public DateTime PriceDate { get; set; }

    public virtual Currency Currency { get; set; }
}