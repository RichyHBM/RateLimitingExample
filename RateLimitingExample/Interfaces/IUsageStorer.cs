using System;

namespace RateLimitingExample
{
	public interface IUsageStorer
	{
		int GetCurrentUsage(string userName);
		bool StoreNewUsage(string userName, int usageCount);
	}
}
	