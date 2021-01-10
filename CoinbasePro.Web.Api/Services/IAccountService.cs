using System.Collections.Generic;
using System.Threading.Tasks;
using CoinbasePro.Core.Dto.Api;

namespace CoinbasePro.Web.Api.Services
{
    public interface IAccountService
    {
        Task<IList<AccountDto>> GetAccounts();
    }
}