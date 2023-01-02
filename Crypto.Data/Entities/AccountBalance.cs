using System;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Annotations;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.Storage.Repository.Core;
using JetBrains.Annotations;

namespace Crypto.Data.Entities;

[UsedImplicitly]
public class AccountBalance : EntityBase
{
    [UsedImplicitly]
    [Column]
    public long AccountId { get; set; }
    
    [UsedImplicitly]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }
    
    [UsedImplicitly]
    [Column]
    public DateTime BalanceDate { get; set; }
    
    [Ignore] 
    public AccountDto Account { get; set; }
}