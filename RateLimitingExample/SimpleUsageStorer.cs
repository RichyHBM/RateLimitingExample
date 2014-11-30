using System;
using System.Timers;
using System.Collections.Generic;

namespace RateLimitingExample
{
	public class SimpleUsageStorer : IUsageStorer
	{
		Timer timer;
		//Ideally you would use redis or some other store mechanism
		Dictionary<string, int> usageList = new Dictionary<string, int>();

		public SimpleUsageStorer (int TimeInSeconds)
		{
			timer = new Timer (TimeInSeconds * 1000);
			timer.Elapsed += WipeUsages;
			timer.AutoReset = true;
			timer.Start ();
		}

		public int GetCurrentUsage (string userName)
		{
			lock (usageList) {
				int usage = 0;
				usageList.TryGetValue (userName, out usage);
				return usage;
			}
		}

		public bool StoreNewUsage (string userName, int usageCount)
		{
			lock (usageList) {
				usageList [userName] = usageCount;
				return true;
			}
		}

		void WipeUsages(object sender, ElapsedEventArgs e)
		{
			lock (usageList) {
				usageList.Clear ();
			}
		}
	}
}

