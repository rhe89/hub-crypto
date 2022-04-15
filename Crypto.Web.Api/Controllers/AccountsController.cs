using System.Threading.Tasks;
using Crypto.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountProvider _accountProvider;

    public AccountsController(IAccountProvider accountProvider)
    {
        _accountProvider = accountProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Accounts([FromQuery]string accountName)
    {
        var accounts = await _accountProvider.GetAccounts(accountName);

        return Ok(accounts);
    }
}