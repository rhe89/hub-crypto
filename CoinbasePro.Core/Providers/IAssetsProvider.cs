using System.Collections.Generic;
using System.Threading.Tasks;
using CoinbasePro.Core.Dto.Data;

namespace Coinbase.Core.Providers
{
    public interface IAssetsProvider
    {
        Task<IList<AssetDto>> GetAssets();
    }
}