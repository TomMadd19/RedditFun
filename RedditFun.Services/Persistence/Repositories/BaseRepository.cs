using RedditFun.Services.Domain.Contexts;

namespace RedditFun.Services.Persistence.Repositories
{
	public abstract class BaseRepository
	{
		protected readonly RedditDataContext _context;

		public BaseRepository(RedditDataContext context)
		{
			_context = context;
		}
	}
}
