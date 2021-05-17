using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoinbasePro.Core.Dto.Data;

namespace CoinbasePro.Core.Providers
{
    public interface IAccountBalanceProvider
    {
        Task<IList<AccountBalanceDto>> GetAssets(string accountName,
            DateTime? fromDate,
            DateTime? toDate);
    }
}