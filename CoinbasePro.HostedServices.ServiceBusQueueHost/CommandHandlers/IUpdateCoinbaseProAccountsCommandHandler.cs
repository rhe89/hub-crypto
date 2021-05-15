using System.Threading.Tasks;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers
{
    public interface IUpdateCoinbaseProAccountsCommandHandler
    {
        Task UpdateAccountAssets();
    }
}