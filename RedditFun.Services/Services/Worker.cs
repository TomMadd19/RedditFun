using RedditFun.Services;
using RedditFun.Services.Domain.Models;

namespace RedditFun.WorkerService
{
	public class Worker : BackgroundService
	{
		private List<string> _subredditList;
		RedditOptions _redditOptions;
		
		public Worker(RedditOptions opts)
		{
			_subredditList = new List<string>();
			_redditOptions = opts;

			_subredditList.Add(_redditOptions.SubReddit);
		}

		public List<string> SubredditList 
		{ 
			get
			{
				return _subredditList;
			}
		}

		public void AddSubReddit(string subReddit)
		{
			try
			{
				lock (_subredditList)
				{
					if (!_subredditList.Contains(subReddit))
					{
						_subredditList.Add(subReddit);
					}
				}

			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
			}
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				Logger.Logs.Info("Worker running at: (" + DateTimeOffset.Now + ")");

				Dictionary<string, Task> threads = new Dictionary<string, Task>();

				foreach (string subReddit in _subredditList)
				{
					RedditService redditReader = new RedditService(subReddit, _redditOptions);

					var cancelToken = new CancellationToken();

					Task t = Task.Factory.StartNew(new Action(redditReader.ReadReddits), 
													cancelToken,
													TaskCreationOptions.LongRunning,
													TaskScheduler.Current);
					threads.Add(subReddit, t);
				}

				foreach (var t in threads)
				{
					Task<string> tRet = t.Value.ContinueWith(c =>
					{
						if (c.IsCompletedSuccessfully)
						{ 
							//Log
							return c.IsCompletedSuccessfully.ToString();
						}
						else if (c.IsFaulted)
						{
							// Log
							return "Error reading subReddits for " + t.Key + " - Error message: " + c.Exception.ToString();
						}
						else if (c.IsCanceled)
						{
							// Log
							return "Reading " + t.Key + " cancelled.";
						}
						else
						{
							return "Reading " + t.Key + " complete.";
						}
					});
				}

				// Wait for completion...
				foreach (var t in threads)
				{
					t.Value.Wait();
				}

				await Task.Delay(60000, cancellationToken);
			}
		}
	}
}
