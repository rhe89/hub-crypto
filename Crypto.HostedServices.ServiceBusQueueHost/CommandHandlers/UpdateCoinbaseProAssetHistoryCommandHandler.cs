using System;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;

public interface IUpdateCoinbaseProAssetHistoryCommandHandler
{
    Task UpdateAccountBalance();
}
    
public class UpdateCoinbaseProAssetHistoryCommandHandler : IUpdateCoinbaseProAssetHistoryCommandHandler
{
    private readonly IHubDbRepository _dbRepository;
    private readonly ILogger<UpdateCoinbaseProAssetHistoryCommandHandler> _logger;

    public UpdateCoinbaseProAssetHistoryCommandHandler(ILogger<UpdateCoinbaseProAssetHistoryCommandHandler> logger,
        IHubDbRepository dbRepository)
    {
        _logger = logger;
        _dbRepository = dbRepository;
    }
        
    public async Task UpdateAccountBalance()
    {
        var accounts = await _dbRepository
            .WhereAsync<Account, AccountDto>(x => x.Exchange == "CoinbasePro");
            
        foreach (var account in accounts)
        {
            var now = DateTime.Now;

            _logger.LogInformation("Updating CoinbasePro-account asset history for account {AccountName}", account.Currency);
                
            var assetHistoryForCurrentDay = GetAssetHistoryForCurrentDay(account, now);
            
            if (assetHistoryForCurrentDay == null)
            {
                AddAssetHistory(account);
            }
            else
            {
                UpdateAssetHistory(assetHistoryForCurrentDay, account);
            }            
        }
            
        await _dbRepository.ExecuteQueueAsync();
            
        _logger.LogInformation("Finished updating CoinbasePro-account balance history");

    }

    private AssetHistoryDto GetAssetHistoryForCurrentDay(AccountDto account, DateTime now)
    {
        return _dbRepository.All<Asset, AssetHistoryDto>().FirstOrDefault(x =>
            x.AccountId == account.Id &&
            x.CreatedDate.Year == now.Year &&
            x.CreatedDate.Month == now.Month &&
            x.CreatedDate.Day == now.Day);
    }
    
    private void AddAssetHistory(AccountDto account)
    {
        var accountBalanceForCurrentDay = new AssetHistoryDto
        {
            AccountId = account.Id,
            Value = (int)account.Balance
        };

        _dbRepository.QueueAdd<Asset, AssetHistoryDto>(accountBalanceForCurrentDay);
    }
        
    private void UpdateAssetHistory(AssetHistoryDto assetHistoryForCurrentDay, AccountDto account)
    {
        assetHistoryForCurrentDay.Value = (int)account.Balance;

        _dbRepository.QueueUpdate<Asset, AssetHistoryDto>(assetHistoryForCurrentDay);
    }

}