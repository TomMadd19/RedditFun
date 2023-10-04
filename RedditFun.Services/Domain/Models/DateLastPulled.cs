
namespace RedditFun.Services.Domain.Models
{
    public class DateLastPulled
    {
		private Dictionary<string, DateTime> _dateDictionary = new Dictionary<string, DateTime>();

		public DateTime this[string key]
		{
			get 
			{ 
				if (!_dateDictionary.ContainsKey(key))
				{
					_dateDictionary.Add(key, DateTime.MinValue);
				}

				return _dateDictionary[key]; 
			}
			set 
			{
				_dateDictionary[key] = value; 
			}
		}
	}

	
}
