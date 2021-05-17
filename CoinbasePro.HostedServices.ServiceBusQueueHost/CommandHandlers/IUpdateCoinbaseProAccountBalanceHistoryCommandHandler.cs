using System.Threading.Tasks;

namespace CoinbasePro.HostedServices.ServiceBusQueueHost.CommandHandlers
{
    public interface IUpdateCoinbaseProAccountBalanceHistoryCommandHandler
    {
        Task UpdateAccountBalance();
    }
}