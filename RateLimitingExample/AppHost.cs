using System;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.Common.Web;

namespace RateLimitingExample
{
	public class AppHost : AppHostHttpListenerBase
	{
		public AppHost () : base( "SimpleService", typeof( AppHost ).Assembly )
		{
		}

		public override void Configure( Funq.Container container )
		{
			Config.DefaultRedirectPath = "/SimpleService";

			//Always return JSON
			RequestFilters.Add ((httpReq, httpResp, requestDto) => {
				httpReq.ResponseContentType = ContentType.Json;
			});

			Config.DebugMode = true;
		}
	}
}
