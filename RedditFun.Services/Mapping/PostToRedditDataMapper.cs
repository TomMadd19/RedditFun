using Reddit.Controllers;
using RedditFun.Services.Domain.Models;

namespace RedditFun.Services.Mapping
{
	public class PostToRedditDataMapper
	{
		public static RedditData MapPostToRedditData(Post p)
		{
			var ret = new RedditData();
			
			ret.Id = p.Id;
			ret.SubReddit = p.Subreddit;
			ret.UpVotes = p.UpVotes;
			ret.DownVotes = p.DownVotes;
			ret.PostedBy = p.Author;
			ret.Score = p.Score;
			ret.FullName = p.Fullname;
			ret.Title = p.Title;
			return ret;
		}
	}
}
