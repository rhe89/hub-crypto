using CoinbasePro.Core.Entities;
using Hub.Storage.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CoinbasePro.Data
{
    public class CoinbaseProDbContext : HostedServiceDbContext
    {
        public CoinbaseProDbContext(DbContextOptions<CoinbaseProDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Account>()
                .ToTable(schema: "dbo", name: "Account");

            builder.Entity<Asset>()
                .ToTable(schema: "dbo", name: "Asset");
        }
    }
}