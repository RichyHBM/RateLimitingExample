using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceHost;

namespace RateLimitingExample
{
	[Route( "/SimpleService" )]
	public class SimpleRequest : IReturn<SimpleResponse>
	{
	}

	public class SimpleResponse
	{
		public string SimpleString { get; set; }
	}

	public class SimpleService : Service
	{
		public SimpleResponse Any(SimpleRequest request)
		{
			return new SimpleResponse ()
			{ 
				SimpleString = "Ok"
			};
		}
	}
}