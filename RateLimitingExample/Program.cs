using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RateLimitingExample
{
    class Program
    {
        static void Main(string[] args)
        {
			AppHost host = new AppHost( );

			host.Init( );
			host.Start( "http://*:8080/" );

			while (true) 
			{
				Thread.Sleep (3600 * 1000);
			}
		}
    }
}
