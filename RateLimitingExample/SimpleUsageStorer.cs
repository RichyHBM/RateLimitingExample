using System;
using System.Timers;
using System.Collections.Generic;

namespace RateLimitingExample
{
	public class SimpleUsageStorer : IUsageStorer
	{
		int TimerTime;

		//Ideally you would use redis or some other store mechanism
		Dictionary<string, IndividualQuotaUsage> usageList = new Dictionary<string, IndividualQuotaUsage>();

		public SimpleUsageStorer (int TimeInSeconds)
		{
			TimerTime = TimeInSeconds;
		}

		public int GetCurrentUsage (string userName)
		{
			int usage = 0;

			IndividualQuotaUsage userUsage = null;
			usageList.TryGetValue (userName, out userUsage);

			if (userUsage != null && userUsage.Active)
				usage = userUsage.Usage;

			return usage;
		}

		public bool StoreNewUsage (string userName, int usageCount)
		{
			if (usageList.ContainsKey (userName) && usageList [userName].Active) {
				usageList [userName].Usage = usageCount;
			} else {
				usageList [userName] = new IndividualQuotaUsage (TimerTime);
				usageList [userName].Usage = usageCount;
			}
			return true;
		}

		//Have a class for each user with its own timer, that way each user can have an individual quota start time
		class IndividualQuotaUsage{

			public int Usage = 0;
			Timer timer;
			public bool Active { get; private set;}

			public IndividualQuotaUsage(int TimeInSeconds)
			{
				timer = new Timer (TimeInSeconds * 1000);
				timer.Elapsed += TimerFinished;
				timer.AutoReset = false;
				timer.Start ();
				Active = true;
			}

			void TimerFinished(object sender, ElapsedEventArgs e)
			{
				Active = false;
			}
		}

	}
}

