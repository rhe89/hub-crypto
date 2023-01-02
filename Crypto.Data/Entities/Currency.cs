using System.ComponentModel.DataAnnotations.Schema;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace Crypto.Data.Entities;

[UsedImplicitly]
public class Currency : EntityBase
{
    [UsedImplicitly]
    [Column]
    public string Name { get; set; }
}