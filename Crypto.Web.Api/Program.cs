using Crypto.Data;
using Hub.Shared.Web.Api;
using Microsoft.Extensions.Hosting;

namespace Crypto.Web.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            .Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return HostBuilder<DependencyRegistrationFactory, CryptoDbContext>
            .Create(args);

    }
}