using Microsoft.EntityFrameworkCore;
using RedditFun.Services.Persistence.Repositories;
using RedditFun.Services.Domain.Models;
using RedditFun.Services.Domain.Contexts;
using RedditFun.Services.Persistence.Interfaces;

namespace RedditFun.WebAPI.Persistence.Repositories
{
	public class CategoryRepository : BaseRepository, IRedditDataRepository
	{
		public CategoryRepository(RedditDataContext context) : base(context)
		{
		}

		public async Task<IEnumerable<RedditData>> ListAsync()
		{
			return await _context.RedditData.ToListAsync();
		}
	}
}
