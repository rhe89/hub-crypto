using System.Collections.Generic;
using System.Threading.Tasks;
using CoinbasePro.Services.Accounts.Models;

namespace CoinbasePro.Core.Integration
{
    public interface ICoinbaseProConnector
    {
        Task<IList<Account>> GetAccounts();
    }
}