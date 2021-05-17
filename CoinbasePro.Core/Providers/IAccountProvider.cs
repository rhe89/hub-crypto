using System.Collections.Generic;
using System.Threading.Tasks;
using CoinbasePro.Core.Dto.Data;

namespace CoinbasePro.Core.Providers
{
    public interface IAccountProvider
    {
        Task<IList<AccountDto>> GetAccounts(string accountName);
    }
}