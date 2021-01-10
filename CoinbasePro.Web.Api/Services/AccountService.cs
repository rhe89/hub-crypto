using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinbase.Core.Providers;
using CoinbasePro.Core.Dto.Api;

namespace CoinbasePro.Web.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountProvider _accountProvider;

        public AccountService(IAccountProvider accountProvider)
        {
            _accountProvider = accountProvider;
        }

        public async Task<IList<AccountDto>> GetAccounts()
        {
            var accounts = await _accountProvider.GetAccounts();
            
            var accountDtos = new List<AccountDto>();

            foreach (var account in accounts)
            {
                var balance = account.Assets
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault();

                var lastMonthBalance = account.Assets
                    .Where(x => x.CreatedDate.Month == DateTime.Now.AddMonths(-1).Month
                                                                 && x.CreatedDate.Year ==
                                                                 DateTime.Now.AddMonths(-1).Year)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault();
                
                var lastYearBalance = account.Assets
                    .Where(x => x.CreatedDate.Year ==
                                DateTime.Now.AddYears(-1).Year)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefault();

                accountDtos.Add(new AccountDto
                {
                    Name = account.Currency,
                    Balance = balance?.Value ?? 0,
                    LastMonthBalance = lastMonthBalance?.Value ?? 0,
                    LastYearBalance = lastYearBalance?.Value ?? 0,
                });
            }

            return accountDtos;
        }
    }
}
