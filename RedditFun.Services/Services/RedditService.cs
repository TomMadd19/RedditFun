using Reddit;
using RedditFun.Services.Domain.Models;
using RedditFun.Services.Controllers;
using RedditFun.Services.Mapping;
using RedditFun.Services.Services;


namespace RedditFun.Services
{
	public class RedditService
	{
		private string _subReddit;
		private RedditOptions _redditOptions;

		public RedditService(string subReddit, RedditOptions opts)
		{
			_subReddit = subReddit;
			_redditOptions = opts;

			if (GlobalVariables.LastPulledDateBySubReddit == null)
			{
				GlobalVariables.LastPulledDateBySubReddit = new Domain.Models.DateLastPulled();
			}
		}

		public void ReadReddits()
		{
			try
			{
				// pull reddits..

				RedditClient client = new RedditClient(_redditOptions.RedditAppID, _redditOptions.RedditRefreshToken, null, _redditOptions.RedditAccessToken, _redditOptions.RedditAccessUser);

				IDictionary<string, IList<Reddit.Controllers.Post>> Posts = new Dictionary<string, IList<Reddit.Controllers.Post>>();

				var subredditList = client.Subreddit(_subReddit).Posts.New.Where(n => n.Created.ToUniversalTime() > GlobalVariables.LastPulledDateBySubReddit[_subReddit].ToUniversalTime());

				if (subredditList == null || subredditList.Count() == 0)
				{
					return;
				}

				foreach (Reddit.Controllers.Post post in subredditList)
				{
					if (!Posts.ContainsKey(post.Subreddit))
					{
						Posts.Add(post.Subreddit, new List<Reddit.Controllers.Post>());
					}
					Posts[post.Subreddit].Add(post);
				}

				GlobalVariables.LastPulledDateBySubReddit[_subReddit] = DateTime.Now;

				// Send the data to RedditStats controller
				var data = new List<RedditData>();

				foreach (Reddit.Controllers.Post p in Posts[_subReddit].ToList())
				{
					data.Add(PostToRedditDataMapper.MapPostToRedditData(p));
				}

				RedditStatsController rc = new RedditStatsController();
				rc.UpdateRedditData(_subReddit, data);

			}
			catch (Exception ex)
			{
				// log error details with _subreddit included
				Logger.Logs.Error(ex.ToString(), ex);
			}
		}

		internal bool IsSubRedditValid()
		{
			try
			{
				RedditClient client = new RedditClient(_redditOptions.RedditAppID, _redditOptions.RedditRefreshToken, null, _redditOptions.RedditAccessToken, _redditOptions.RedditAccessUser);
				var about = client.Subreddit(_subReddit).About();

				// check if Subreddit is vaild
				if (about != null)
				{
					return !String.IsNullOrEmpty(about.URL.Trim());
				}

				return false;
			}
			catch (Exception ex) 
			{
				Logger.Logs.Error(ex.ToString(), ex);

				return false;
			}
		}
	}
}
