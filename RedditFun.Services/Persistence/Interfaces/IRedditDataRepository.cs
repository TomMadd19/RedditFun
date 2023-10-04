using RedditFun.Services.Domain.Models;


namespace RedditFun.Services.Persistence.Interfaces
{
    public interface IRedditDataRepository
    {
        Task<IEnumerable<RedditData>> ListAsync();
    }
}
