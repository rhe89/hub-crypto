using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace Crypto.Data.Entities;

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
    [Column]
    public string Exchange { get; set; }

    [UsedImplicitly]
    public ICollection<Asset> Assets { get; set; }
}