using System;
using NUnit.Framework;
using System.Net;
using System.Collections.Specialized;
using System.Threading;

namespace RateLimitingExample
{
	[TestFixture]
	public class IntegrationTest
	{
		AppHost host;

		[SetUp] 
		public void Init()
		{
			//For testing purposes the quota limit will reset every 5 seconds
			host = new AppHost( 5 );
			host.Init( );
			host.Start( "http://*:8080/" );
		}

		[TearDown] 
		public void Cleanup()
		{
			host.Stop();
			host.Dispose ();
		}

		[Test]
		public void DenyInvalidUserTest()
		{
			Assert.IsFalse( MakeSimpleServiceRequest ("aUser", "") );
		}

		[Test]
		public void AllowValidUserTest()
		{
			Assert.IsTrue( MakeSimpleServiceRequest ("User01", "SecretUser01") );
		}

		[Test]
		public void AllowQuotaUserTest()
		{
			//User01 only allows 10 requests per x time
			for( int i = 0; i < 10; i++)
			{
				Assert.IsTrue( MakeSimpleServiceRequest ("User01", "SecretUser01") );
			}
			Assert.IsFalse( MakeSimpleServiceRequest ("User01", "SecretUser01") );
		}

		[Test]
		public void CorrectlyResetsQuotaTest()
		{
			while( MakeSimpleServiceRequest ("UpgradedUser01", "SecretUpgradedUser01"));

			Assert.IsFalse( MakeSimpleServiceRequest ("UpgradedUser01", "SecretUpgradedUser01") );

			while (!MakeSimpleServiceRequest ("UpgradedUser01", "SecretUpgradedUser01")) {
				Thread.Sleep (5 * 1000);
			}

			Assert.IsTrue( MakeSimpleServiceRequest ("UpgradedUser01", "SecretUpgradedUser01") );
		}

		[Test]
		public void AllowUnlimitedForAdminTest()
		{
			for( int i = 0; i < 100; i++)
			{
				Assert.IsTrue( MakeSimpleServiceRequest ("Admin", "SecretAdmin") );
			}
		}

		public bool MakeSimpleServiceRequest (string userName, string apiKey)
		{
			using (WebClient client = new WebClient())
			{
				client.Headers.Add("Username", userName);
				client.Headers.Add("ApiKey", apiKey);

				try{
					String response = client.DownloadString("http://127.0.0.1:8080/SimpleService");
					return response == "Ok";
				}catch(WebException) {
					return false;
				}
			}
		}
	}
}

