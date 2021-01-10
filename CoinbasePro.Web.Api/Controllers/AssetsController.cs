using System.Threading.Tasks;
using CoinbasePro.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsService _assetsService;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetsService assetsService, ILogger<AssetsController> logger)
        {
            _assetsService = assetsService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Assets()
        {
            var assets = await _assetsService.GetAssets();

            return Ok(assets);
        }
    }
}
