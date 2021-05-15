using System.Threading.Tasks;
using CoinbasePro.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinbasePro.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsService _assetsService;

        public AssetsController(IAssetsService assetsService)
        {
            _assetsService = assetsService;
        }

        [HttpGet]
        public async Task<IActionResult> Assets()
        {
            var assets = await _assetsService.GetAssets();

            return Ok(assets);
        }
    }
}
