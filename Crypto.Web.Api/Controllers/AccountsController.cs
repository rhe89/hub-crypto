using System.Threading.Tasks;
using Crypto.Providers;
using Hub.Shared.DataContracts.Crypto.SearchParameters;
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

    [HttpPost]
    public async Task<IActionResult> Accounts(AccountSearchParameters accountSearchParameters)
    {
        var accounts = await _accountProvider.GetAccounts(accountSearchParameters);

        return Ok(accounts);
    }
}