using System;
using NUnit.Framework;

namespace RateLimitingExample
{
	[TestFixture]
	public class QuotaProviderTest
	{
		IQuotaProvider provider;

		[SetUp] 
		public void Init()
		{
			provider = new SimpleQuotaProvider ();
		}

		[TearDown] 
		public void Cleanup()
		{
			provider = null;
		}

		[Test]
		public void AuthenticateUserTest ()
		{
			Assert.IsTrue(provider.AuthenticateUser("User01", "SecretUser01"));
		}

		[Test]
		public void WrongSecretTest ()
		{
			Assert.IsFalse(provider.AuthenticateUser("User01", "someString"));
		}

		[Test]
		public void InvalidUserTest ()
		{
			Assert.IsFalse(provider.AuthenticateUser("aUser", "someString"));
		}

		[Test]
		public void GetHourlyQuotaTest ()
		{
			Assert.AreEqual(10, provider.GetHourlyQuota("User01"));
			Assert.AreEqual(20, provider.GetHourlyQuota("UpgradedUser01"));
			Assert.AreEqual(-1, provider.GetHourlyQuota("Admin"));
		}

		[Test]
		public void GetHourlyQuotaUnknownUserTest ()
		{
			try{
				provider.GetHourlyQuota("aUser");
				Assert.IsTrue(false);
			}catch(Exception) {
				Assert.IsTrue (true);
			}
		}
	}
}

