# RedditFun

Reads Subreddits and reports on console app.

I called this RedditFun.  It was a fun challenge.

There are 2 main projects:
1) RedditFun - console app that allows 3 actions (Show stats from SubReddits, Add a new SubReddit to monitor, or close the program.

2) RedditFun.Services - contains 3 services 
	1. Processor - Worker Service hosted as a Windows Service, this handles the polling of the subreddit feeds
	2. ReditService - called by the worker service to enable multithreading (1 thread for each subreddit being monitored)
	3. Web API to provide the data report to the console application.

Configurations to know about:
1) Reddit API settings - 
	In appsettings.json file in RedditFun.Services project:
	"RedditConfig": 
	{
	  "RedditAppID": Reddit API Application ID
	  "RedditRefreshToken":Reddit Refresh Token
	  "RedditAccessToken": Reddit Access Token 
	  "RedditAccessUser": Reddit API user name
	  "SubReddit": initial subreddit to monitor by default
	}

2) In RedditFun console application - appsettings.json contains the URL of the Web API service.  It defaults to localhost:5001
	
3) In log4netconfig.config - main projects have this for logging settings.

Everything else should be straight forward and easy to follow.
