using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceHost;

namespace RateLimitingExample
{
	[Route( "/SimpleService" )]
	public class SimpleRequest : IReturn<string>
	{
	}

	public class SimpleService : Service
	{
		public string Any(SimpleRequest request)
		{
			return "Ok";
		}
	}
}