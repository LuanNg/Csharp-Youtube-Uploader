using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;

namespace Csharp_Youtube_Uploader
{
	[Serializable]
	class Youtube_request
	{
		public static YouTubeService getYoutubeService(UserCredential credential)
		{
			return new YouTubeService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "C-Sharp Youtube Uploader"
			});
		}
	}
}
