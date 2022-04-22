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
            .ToTable(schema: "dbo", name: "Account");

        builder.Entity<Asset>()
            .ToTable(schema: "dbo", name: "Asset");
        
        builder.Entity<ExchangeRate>()
            .ToTable(schema: "dbo", name: "ExchangeRate");
    }
}