using System;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.Common.Web;

namespace RateLimitingExample
{
	public class AppHost : AppHostHttpListenerBase
	{
		//Wipe the usage store every hour
		int UsageTimeToLive = 3600;

		public AppHost () : base( "SimpleService", typeof( AppHost ).Assembly )
		{
		}

		public AppHost (int usageTimeToLive) : base( "SimpleService", typeof( AppHost ).Assembly )
		{
			UsageTimeToLive = usageTimeToLive;
		}

		public override void Configure( Funq.Container container )
		{
			Config.DefaultRedirectPath = "/SimpleService";

			//Register the quota plugin
			Plugins.Add(new SimpleQuotaPlugin(
				new SimpleQuotaProvider(),
				new SimpleUsageStorer(UsageTimeToLive) 
			));

			//Always return JSON
			RequestFilters.Add ((httpReq, httpResp, requestDto) => {
				httpReq.ResponseContentType = ContentType.Json;
			});
				
			Config.DebugMode = true;
		}
	}
}
