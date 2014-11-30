using System;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceHost;

namespace RateLimitingExample
{
	public class SimpleQuotaPlugin : IPlugin
	{
		IQuotaProvider QuotaProvider;
		IUsageStorer UsageStorer;

		public SimpleQuotaPlugin(IQuotaProvider quotaProvider, IUsageStorer usageStorer)
		{
			QuotaProvider = quotaProvider;
			UsageStorer = usageStorer;
		}

		public void Register (IAppHost host)
		{
			host.RequestFilters.Add (this.FilterRequests);
		}

		void FilterRequests (IHttpRequest request, IHttpResponse response, object requestObject)
		{
			string userName = request.Headers.Get ("Username");
			string apiKey = request.Headers.Get ("ApiKey");

			if (String.IsNullOrEmpty (userName)) {
				throw new Exception ("Please suply username in 'Username' header");
			}

			if (String.IsNullOrEmpty (apiKey)) {
				throw new Exception ("Please suply API key in 'ApiKey' header");
			}

			if (!QuotaProvider.AuthenticateUser (userName, apiKey)) {
				throw new Exception ("Invalid user or API key");
			}

			int quotaAmount = QuotaProvider.GetHourlyQuota (userName);

			if (quotaAmount >= 0) {
				int currentUsage = UsageStorer.GetCurrentUsage (userName);

				//Increment for each API call
				currentUsage++;

				if (currentUsage > quotaAmount) {
					throw new Exception ("You have exceded your quota!");
				}

				UsageStorer.StoreNewUsage (userName, currentUsage);
			}
		}
	}
}

