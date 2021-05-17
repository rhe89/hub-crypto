using CoinbasePro.Core.Entities;
using Hub.Storage.Repository;
using Microsoft.EntityFrameworkCore;

namespace CoinbasePro.Data
{
    public class CoinbaseProDbContext : HubDbContext
    {
        public CoinbaseProDbContext(DbContextOptions<CoinbaseProDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Account>()
                .ToTable(schema: "dbo", name: "Account");

            builder.Entity<AccountBalance>()
                .ToTable(schema: "dbo", name: "Asset");
        }
    }
}