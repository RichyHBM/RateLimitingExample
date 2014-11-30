using System;

namespace RateLimitingExample
{
	public interface IQuotaProvider
	{
		bool AuthenticateUser(string UserName, string ApiKey);
		int GetHourlyQuota(string UserName);
	}
}

