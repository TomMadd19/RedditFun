using Microsoft.AspNetCore.Mvc;
using RedditFun.Interfaces;
using RedditFun.Services.Domain.Models;
using RedditFun.Services.Services;

namespace RedditFun.Services.Controllers
{
	[ApiController]
	[Route("Processor/{action}")]
	public class ProcessorController : Controller
	{
		
		public ProcessorController()
		{
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		public ServiceResponse AddSubReddit(string subReddit)
		{
			try
			{
				RedditOptions options = GlobalVariables.RedditOptions;
				// validate subReddit
				RedditService serv = new RedditService(subReddit, options);
				bool ret = serv.IsSubRedditValid();

				if (ret)
				{
					GlobalVariables.WorkerThread.AddSubReddit(subReddit);

					return new ServiceResponse("Success", "Successfully added SubReddit '" + subReddit + "'");
				}
				else
				{
					return new ServiceResponse("Failure", "Could not connect to SubReddit '" + subReddit + "'");
				}
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);

				return new ServiceResponse("Failure", subReddit + " not added due to following error: " + ex.ToString());
			}
		}
	}
}
