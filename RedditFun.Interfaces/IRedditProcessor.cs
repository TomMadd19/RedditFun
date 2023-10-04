
namespace RedditFun.Interfaces
{
	public interface IRedditProcessor : IService
	{
		ServiceResponse AddReddit(string data);
	}
}
