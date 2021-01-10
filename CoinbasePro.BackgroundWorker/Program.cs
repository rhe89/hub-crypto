using CoinbasePro.Data;
using Hub.HostedServices.TimerHost;
using Microsoft.Extensions.Hosting;

namespace CoinbasePro.BackgroundWorker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new BackgroundWorker<DependencyRegistrationFactory, CoinbaseProDbContext>(args,"SQL_DB_COINBASE_PRO")
                .CreateHostBuilder()
                .Build()
                .Run();
        }
    }
}