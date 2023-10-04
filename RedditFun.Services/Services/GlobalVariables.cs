using RedditFun.Services.Domain.Models;
using RedditFun.WorkerService;

namespace RedditFun.Services.Services
{
	public static class GlobalVariables
	{
		public static RedditOptions RedditOptions { get; set; }
		public static Worker WorkerThread { get; set; }

		public static DateLastPulled LastPulledDateBySubReddit { get; set; }
		
	}
}
