using CoinbasePro.Data;
using Microsoft.Extensions.Hosting;
using Hub.Web.Api;

namespace CoinbasePro.Web.Api
{
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
            return HostBuilder<DependencyRegistrationFactory, CoinbaseProDbContext>
                .Create(args);

        }
    }
}
