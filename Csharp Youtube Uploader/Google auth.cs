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
		/// <summary>
		/// Gets a User credential :)
		/// </summary>
		/// <returns>UserCredential</returns>
		static async Task<UserCredential> getUserCredential()
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
	}
}
