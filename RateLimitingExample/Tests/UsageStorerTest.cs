using System;
using NUnit.Framework;
using System.Threading;

namespace RateLimitingExample
{
	public class UsageStorerTest
	{
		IUsageStorer storer;

		[SetUp] 
		public void Init()
		{
			storer = new SimpleUsageStorer (2);
		}

		[TearDown] 
		public void Cleanup()
		{
			storer = null;
		}

		[Test]
		public void GetInvalidUsageTest()
		{
			Assert.AreEqual (0, storer.GetCurrentUsage ("foo"));
		}

		[Test]
		public void StoreAndRetreiveUsageTest()
		{
			Assert.AreEqual (0, storer.GetCurrentUsage ("foo"));
			storer.StoreNewUsage ("foo", 10);
			Assert.AreEqual (10, storer.GetCurrentUsage ("foo"));
		}

		[Test]
		public void WipeUsageTest()
		{
			storer.StoreNewUsage ("foo", 10);
			Assert.AreEqual (10, storer.GetCurrentUsage ("foo"));
			Thread.Sleep (5000);
			Assert.AreEqual (0, storer.GetCurrentUsage ("foo"));
		}
	}
}

