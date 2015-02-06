using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
namespace Csharp_Youtube_Uploader
{
	class video_constructor
	{
		public static Video constructVideo(string Title, string Description, string[] tags,string PrivacyStatus)
		{
			var video = new Video();
			video.Snippet = new VideoSnippet();
			video.Snippet.Title = Title;
			video.Snippet.Description = Description;
			video.Snippet.Tags = tags;
			video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list ---> Find ich nicht
			video.Status = new VideoStatus();
			video.Status.PrivacyStatus = PrivacyStatus; // or "private" or "public"
			return video;
		}
	}
}
