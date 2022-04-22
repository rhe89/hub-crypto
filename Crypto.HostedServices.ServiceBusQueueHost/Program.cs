using Crypto.Data;
using Hub.Shared.HostedServices.ServiceBusQueue;
using Microsoft.Extensions.Hosting;

namespace Crypto.HostedServices.ServiceBusQueueHost;

public static class Program
{
    public static void Main(string[] args)
    {
        new Bootstrapper<DependencyRegistrationFactory, CryptoDbContext>(args, "SQL_DB_CRYPTO")
            .CreateHostBuilder()
            .Build()
            .Run();
    }
}