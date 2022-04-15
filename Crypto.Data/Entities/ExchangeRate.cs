using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace Crypto.Data.Entities;

[UsedImplicitly]
public class ExchangeRate : EntityBase
{
    [UsedImplicitly]
    [Column]
    public string Currency { get; set; }
        
    [UsedImplicitly]
    [Column]
    public decimal NOKRate { get; set; }
        
    [UsedImplicitly]
    [Column]
    public decimal USDRate { get; set; }
        
    [UsedImplicitly]
    [Column]
    public decimal EURRate { get; set; }
}