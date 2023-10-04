using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RedditFun.Interfaces;


namespace RedditFun
{
	internal class Program
	{
		protected static IConfigurationRoot? _config;

		private static void Main(string[] args)
		{
			try
			{
				SetConfig();

				bool done = false;

				// Console App has 3 options Show Stats (S), Add SubReddit (A), Exit  (X)
				Console.WriteLine("This is a Reddit Post reporting service.");
				Console.WriteLine("You have 3 options:");
				Console.WriteLine("Show Stats (S), Add SubReddit (A), Exit  (X).");

				while (!done)
				{
					var ret = Console.ReadLine();

					switch (ret)
					{
						case "s":
						case "S":
							var stats = GetRedditStats();
							Console.WriteLine(stats);
							break;
						case "a":
						case "A":
							Console.WriteLine("What SubReddit would you like to add?");
							ret = Console.ReadLine();

							var added = AddSubReddit(ret);
							Console.WriteLine(added);
							break;
						case "x":
						case "X":
							done = true;
							break;

						default:
							Console.WriteLine("Invalid Selection!");
							break;
					}
						
					if (!done)
					{
						Console.WriteLine("Show Stats (S), Add SubReddit (A), Exit  (X).");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
				Console.WriteLine("Fatal error: " + ex.ToString());
			}
		}

		static void SetConfig()
		{
			var builder = new ConfigurationBuilder()
				 .AddJsonFile($"appsettings.json", true, true);

			_config = builder.Build();
		}

		protected static string GetRedditStats()
		{
			try
			{
				var json = CallRestAPI("GetRedditStats", "subReddit");
				var data = json.Result.ResponseData;

				var resp = JsonConvert.DeserializeObject<ServiceResponse>(data);

				return resp.ResponseData;
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
				throw;
			}
		}

		protected static string AddSubReddit(string subreddit)
		{
			try
			{
				var resp = CallRestAPI("AddReddit", subreddit);

				return resp.Result.ResponseData;
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
				throw;
			}
		}

		protected static async Task<ServiceResponse?> CallRestAPI(string action, string data)
		{
			try
			{
				using (var httpClient = new HttpClient())
				{
					using (var response = await httpClient.GetAsync(_config["appSettings:WebAPIBaseURL"] + action + "/" + data))
					{
						string apiResponse = await response.Content.ReadAsStringAsync();
						return new ServiceResponse("success", apiResponse);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Logs.Error(ex.ToString(), ex);
				return new ServiceResponse("failed", ex.ToString());
			}
		}
	}
}