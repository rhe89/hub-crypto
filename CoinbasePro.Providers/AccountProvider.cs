using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Providers;
using CoinbasePro.Core.Dto.Data;
using CoinbasePro.Core.Entities;
using Hub.Storage.Repository.Core;
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
            var accounts = await _dbRepository
                .AllAsync<Account, AccountDto>(source => source
                    .Include(x => x.Assets));

            return accounts;


        }
    }
}
