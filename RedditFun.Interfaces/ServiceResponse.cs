
namespace RedditFun.Interfaces
{
	public sealed class ServiceResponse
	{

		public ServiceResponse() { }
		public ServiceResponse(string responseStatus, string responseData)
		{
			ResponseStatus = responseStatus;
			ResponseData = responseData;
		}

		internal void SetResponseStatus(string status)
		{
			ResponseStatus = status;
		}

		public string ResponseStatus { get; set; }


		public string ResponseData { get; set; }

	}
}
