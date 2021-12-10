using CoinbasePro.Data;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Microsoft.Extensions.Hosting;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new Bootstrapper<DependencyRegistrationFactory, CoinbaseProDbContext>(args, 
                    "SQL_DB_COINBASE_PRO")
                .CreateHostBuilder()
                .Build()
                .Run();
        }
    }
}