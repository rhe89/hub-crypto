using System.Collections.Generic;
using System.Threading.Tasks;
using Coinbase.Core.Providers;
using CoinbasePro.Core.Dto.Data;
using CoinbasePro.Core.Entities;
using Hub.Storage.Repository.Core;
using Microsoft.EntityFrameworkCore;

namespace CoinbasePro.Providers
{
    public class AssetsProvider : IAssetsProvider
    {
        private readonly IHubDbRepository _dbRepository;

        public AssetsProvider(IHubDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task<IList<AssetDto>> GetAssets()
        {
            var assets = await _dbRepository
                .AllAsync<Asset, AssetDto>(source => source.Include(x => x.Account));

            return assets;
        }
    }
}
