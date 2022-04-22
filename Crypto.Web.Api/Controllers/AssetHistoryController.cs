using System.Threading.Tasks;
using Crypto.Providers;
using Hub.Shared.DataContracts.Crypto.SearchParameters;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetHistoryController : ControllerBase
{
    private readonly IAssetHistoryProvider _assetHistoryProvider;

    public AssetHistoryController(IAssetHistoryProvider assetHistoryProvider)
    {
        _assetHistoryProvider = assetHistoryProvider;
    }

    [HttpPost]
    public async Task<IActionResult> GetAssetHistory(AssetHistorySearchParameters assetHistorySearchParameters)
    {
        var assetHistory = await _assetHistoryProvider.GetAssetHistory(assetHistorySearchParameters);

        return Ok(assetHistory);
    }
}