using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading.Tasks;
using System.Threading;

namespace Csharp_Youtube_Uploader
{
	class Google_auth
	{
		static UserCredential userCred;
		private static async Task<UserCredential> requestUserCredential()
		{
			
				return  await GoogleWebAuthorizationBroker.AuthorizeAsync(
					new ClientSecrets
					{
						ClientId = "931649744860-vbhhdcqmahkkltvj84otg8t8a2iffqlq.apps.googleusercontent.com",
						ClientSecret = "Aj1OCCqh8Qgp2sVYZAQwZKm9"
					}, 
					new[] { BooksService.Scope.Books },
					"user",
					CancellationToken.None,
					new FileDataStore("Books.YoutubeLibrary"));

					
				
			
		}
		
		public delegate void OnReceivedUserCredential();
		/// <summary>
		/// Gets fired when the usercred is received
		/// </summary>
		public static event OnReceivedUserCredential Check;
		private static void eventEnabler()
		{
			if (Check != null)
			{
				Check();
			}
		}
		private static async void getUserCred()
		{
			userCred = await requestUserCredential();
			eventEnabler();
		}
		public static UserCredential returnUserCredential(){
			return userCred;
		}

	}
}
