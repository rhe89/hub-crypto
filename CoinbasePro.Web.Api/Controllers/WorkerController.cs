using CoinbasePro.BackgroundTasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Core.Providers;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CoinbasePro.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : WorkerControllerBase
    {
        private readonly IBackgroundTaskQueueHandler _backgroundTaskQueueHandler;

        public WorkerController(IWorkerLogProvider workerLogProvider, 
            IBackgroundTaskQueueHandler backgroundTaskQueueHandler) : base(workerLogProvider)
        {
            _backgroundTaskQueueHandler = backgroundTaskQueueHandler;
        }
        
        [HttpPost("QueueUpdateAccountsTask")]
        public IActionResult QueueUpdateAccountsTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateAccountsTask>();

            return Ok();
        }
    }
}