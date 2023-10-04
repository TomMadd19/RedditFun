using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedditFun.Interfaces;
using RedditFun.Services.Domain.Contexts;
using RedditFun.Services.Domain.Models;
using RedditFun.Services.Services;

namespace RedditFun.Services.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RedditStatsController : ControllerBase
	{
		public RedditStatsController()
		{
			Logger.Logs.Info("Starting Reddit Web API");
		}

		[HttpGet("[action]/{data}")]
		public ServiceResponse GetRedditStats(string data)
		{
			try
			{
				string outputData = GetRedditStatsReport();

				return new ServiceResponse("Sucess", outputData);
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);

				return new ServiceResponse("Failure", ex.ToString());
			}
		}

		private string GetRedditStatsReport()
		{
			string totalSubreddits;
			string userWithMostPosts;
			string PostWithMostUpvotes;
			string PostWithMostDownvotes;

			string returnString = "";

			// Get Data from EF in memory...
			DbContextOptions<RedditDataContext> opts = new DbContextOptions<RedditDataContext>();
			using (RedditDataContext db = new RedditDataContext(opts))
			{
				List<string> subRedditList = GlobalVariables.WorkerThread.SubredditList;

				int i = 0;

				foreach (string subReddit in subRedditList)
				{
					List<RedditData> _redditList = new List<RedditData>();

					// total subreddits posted
					totalSubreddits = TotalSubreddits(db, subReddit);

					// Most upvotes..
					PostWithMostUpvotes = MostUpVotes(db, subReddit);

					// Most downvotes..
					PostWithMostDownvotes = MostDownVotes(db, subReddit);

					// Users with most posts ..
					userWithMostPosts = UserMostPosts(db, subReddit);

					if (i == 0)
					{
						returnString += "Stats for SubReddit '" + subReddit + "':" + Environment.NewLine + totalSubreddits + Environment.NewLine + PostWithMostUpvotes + Environment.NewLine + PostWithMostDownvotes + Environment.NewLine + userWithMostPosts;
						i = 1;
					}
                    else
                    {
						returnString += Environment.NewLine + Environment.NewLine + "Stats for SubReddit '" + subReddit + "':" + Environment.NewLine + totalSubreddits + Environment.NewLine + PostWithMostUpvotes + Environment.NewLine + PostWithMostDownvotes + Environment.NewLine + userWithMostPosts;
					}
                }
			}
				
			return returnString;
		}
		private string TotalSubreddits(RedditDataContext db, string subReddit)
		{
			try
			{
				var count = db.RedditData.Where(d => d.SubReddit == subReddit).Count();

				return "Total Subreddits: " + count.ToString();
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
				return "Total Subreddits: N/A";
			}
		}

		private string MostUpVotes(RedditDataContext db, string subReddit)
		{
			try
			{
				var upvotes = db.RedditData.Where(d => d.SubReddit == subReddit).ToList().OrderByDescending(l => l.UpVotes).FirstOrDefault();

				if (upvotes != null)
				{
					if (!String.IsNullOrEmpty(upvotes.FullName))
					{
						return "Most upvotes are for post " + upvotes.FullName + " with " + upvotes.UpVotes.ToString();
					}
					else
					{
						return "No SubReddits have loaded yet for '" + subReddit + "'";
					}
				}
				else
				{
					return "No SubReddits have loaded yet for '" + subReddit + "'";
				}
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
				return "Subreddits cannot be read.";
			}
		}

		private string MostDownVotes(RedditDataContext db, string subReddit)
		{
			try
			{
				var downvotes = db.RedditData.Where(d => d.SubReddit == subReddit).ToList().OrderByDescending(l => l.DownVotes).FirstOrDefault();

				if (downvotes != null)
				{
					if (!String.IsNullOrEmpty(downvotes.FullName))
					{
						return "Most downvotes are for post " + downvotes.FullName + " with " + downvotes.DownVotes.ToString();
					}
					else
					{
						return "No SubReddits have loaded yet for '" + subReddit + "'";
					}
				}
				else
				{
					return "No SubReddits have loaded yet for '" + subReddit + "'";
				}
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
				return "Subreddits cannot be read.";
			}
		}

		private string UserMostPosts(RedditDataContext db, string subReddit)
		{
			try
			{
				var userMostPosts = db.RedditData.Where(d => d.SubReddit == subReddit).ToList().GroupBy(g => g.PostedBy).OrderByDescending(o => o.Count()).ToDictionary(g => g.Key, g => g.Count()).FirstOrDefault();

				if (!String.IsNullOrEmpty(userMostPosts.Key))
				{
					return "The user with the most posts is " + userMostPosts.Key + " with " + userMostPosts.Value.ToString() + " posts.";
				}
				else
				{
					return "No SubReddits have loaded yet for '" + subReddit + "'";
				}
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
				return "Subreddits cannot be read.";
			}
		}

		[HttpGet("[action]/{data}")]
		public ServiceResponse AddReddit(string data)
		{
			try
			{
				ProcessorController pc = new ProcessorController();
				
				return pc.AddSubReddit(data);
			}
			catch (Exception ex)
			{
				Logger.Logs.Error("Adding SubReddit " + data + " failed due to excepition: " + ex.ToString(), ex);

				return new ServiceResponse("Failure", "'" + data + "'" + " not added.  Error: " + ex.ToString());
			}
		}

		internal ServiceResponse UpdateRedditData(string subReddit, List<RedditData> data)
		{
			try
			{
				// Update EF from data provided in req..
				DbContextOptions<RedditDataContext> opts = new DbContextOptions<RedditDataContext>();
				using (RedditDataContext db = new RedditDataContext(opts))
				{
					db.RedditData.AddRange(data);
					db.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);

				return new ServiceResponse("Failure", "Data update for " + subReddit + " Error: " + ex.ToString());
			}

			return new ServiceResponse("Completed Successfully", "Data update for " + subReddit + " succeful.");
		}
	}
}
