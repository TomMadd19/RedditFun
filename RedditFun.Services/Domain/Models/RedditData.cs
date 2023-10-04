using System.ComponentModel.DataAnnotations;

namespace RedditFun.Services.Domain.Models
{
	public class RedditData
	{
		public RedditData() 
		{

		}

		public string Id { get; set; }
		public string SubReddit { get; set; }

		[MaxLength(50)]
		public string FullName { get; set; }
		public string Title { get; set; }
		public int UpVotes { get; set; }
		public int DownVotes { get; set; }
		[MaxLength(50)]
		public string PostedBy { get; set; }
		public int Score { get; set; }

	}
}
