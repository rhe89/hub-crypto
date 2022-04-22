using System;
using System.Linq;
using System.Threading.Tasks;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;
using Hub.Shared.Storage.Repository.Core;
using Microsoft.Extensions.Logging;

namespace Crypto.HostedServices.ServiceBusQueueHost.CommandHandlers;

public interface IUpdateCoinbaseAssetHistoryCommandHandler
{
    Task UpdateAssetHistory();
}
    
public class UpdateCoinbaseAssetHistoryCommandHandler : IUpdateCoinbaseAssetHistoryCommandHandler
{
    private readonly IHubDbRepository _dbRepository;
    private readonly ILogger<UpdateCoinbaseAssetHistoryCommandHandler> _logger;

    public UpdateCoinbaseAssetHistoryCommandHandler(ILogger<UpdateCoinbaseAssetHistoryCommandHandler> logger,
        IHubDbRepository dbRepository)
    {
        _logger = logger;
        _dbRepository = dbRepository;
    }
        
    public async Task UpdateAssetHistory()
    {
        var accountsInDb = await _dbRepository
            .WhereAsync<Account, AccountDto>(x => x.Exchange == "Coinbase");
        
        foreach (var account in accountsInDb)
        {
            var now = DateTime.Now;

            _logger.LogInformation("Updating Coinbase-account asset history for account {Account}", account.Currency);
                
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
            
        _logger.LogInformation("Finished updating Coinbase-account balance history");

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