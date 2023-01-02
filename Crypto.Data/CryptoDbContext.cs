using Crypto.Data.Entities;
using Hub.Shared.Storage.Repository;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Data;

public class CryptoDbContext : HubDbContext
{
    public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options) { }
        
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Account>()
            .ToTable(schema: "dbo", name: "AccountTmp");
        
        builder.Entity<AccountBalance>()
            .ToTable(schema: "dbo", name: "AccountBalance");
        
        builder.Entity<Currency>()
            .ToTable(schema: "dbo", name: "Currency");
        
        builder.Entity<CurrencyPrice>()
            .ToTable(schema: "dbo", name: "CurrencyPrice");

        builder.Entity<Wallet>()
            .ToTable(schema: "dbo", name: "Wallet");
    }
}