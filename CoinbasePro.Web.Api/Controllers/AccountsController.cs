using System.Threading.Tasks;
using CoinbasePro.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinbasePro.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Accounts()
        {
            var accounts = await _accountService.GetAccounts();

            return Ok(accounts);
        }
    }
}
