using CoinbasePro.Data;
using Hub.HostedServices.Schedule;
using Microsoft.Extensions.Hosting;

namespace CoinbasePro.HostedServices.ScheduledHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new Bootstrapper<DependencyRegistrationFactory, CoinbaseProDbContext>(args,"SQL_DB_COINBASE_PRO")
                .CreateHostBuilder()
                .Build()
                .Run();
        }
    }
}