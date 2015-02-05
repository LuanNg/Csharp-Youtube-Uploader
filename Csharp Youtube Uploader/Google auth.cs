using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace Csharp_Youtube_Uploader
{
	class Google_auth
	{
		static UserCredential userCred;
		public static async Task<UserCredential> requestUserCredentialUpload()
		{
				MessageBox.Show("Task will be returned");
				return await GoogleWebAuthorizationBroker.AuthorizeAsync(
					new ClientSecrets
					{
						ClientId = "931649744860-vbhhdcqmahkkltvj84otg8t8a2iffqlq.apps.googleusercontent.com",
						ClientSecret = "Aj1OCCqh8Qgp2sVYZAQwZKm9"
					}, 
					new[] { YouTubeService.Scope.YoutubeUpload, YouTubeService.Scope.Youtube, YouTubeService.Scope.Youtubepartner },
					"user",
					CancellationToken.None,
					new FileDataStore("C#YTUploader/Youtube.Auth.Store")
					);
				

					
				
			
		}
		
		
		

	}
}
