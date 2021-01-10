using System;
using System.Threading.Tasks;
using CoinbasePro.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoinbasePro.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> Accounts()
        {
            var accounts = await _accountService.GetAccounts();

            return Ok(accounts);
        }
    }
}
