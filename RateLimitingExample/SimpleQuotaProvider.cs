using System;
using System.Collections.Generic;

namespace RateLimitingExample
{
	//Ideally this should be retreived from a database or some other store
	public class SimpleQuotaProvider : IQuotaProvider
	{
		Dictionary<String, int> usersQuota = new Dictionary<String, int> () {
			{ "User01", 10 }, 
			{ "UpgradedUser01", 20 }, 
			{ "Admin", -1 } //-1 will indicate no quota limit
		};

		public bool AuthenticateUser (string UserName, string ApiKey)
		{
			//Just check if the user exists, 
			//in this simple version the ApiKey will just be Secret123 where 123 is the user name
			//EG, user01 will be Secretuser1
			return usersQuota.ContainsKey (UserName) && ApiKey == String.Format ("Secret{0}", UserName);
		}

		public int GetHourlyQuota (string UserName)
		{
			if (!usersQuota.ContainsKey (UserName))
				throw new Exception ("User not found");
			return usersQuota [UserName];
		}
	}
}

