using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Providers;
using CoinbasePro.Core.Dto.Data;
using CoinbasePro.Core.Entities;
using Hub.Storage.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace CoinbasePro.Providers
{
    public class AccountProvider : IAccountProvider
    {
        private readonly IHubDbRepository _dbRepository;

        public AccountProvider(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<IList<AccountDto>> GetAccounts()
        {
            var accounts = _dbRepository
                .Set<Account>()
                .Include(x => x.Assets);

            return await _dbRepository.ProjectAsync<Account, AccountDto>(accounts);


        }
    }
}
