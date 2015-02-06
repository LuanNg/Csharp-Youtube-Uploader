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
using System.Windows;
namespace Csharp_Youtube_Uploader
{
	class video_constructor
	{
		public enum Categories
		{
			Film = 1,
			Vehicles = 2,
			Music = 10,
			Animals = 15,
			ShortMovie = 18,
			Events = 19,
			Gaming = 20,
			VLOG = 21,
			PeopleandBlogs = 22,
			Comedy = 23,
			Entertainment = 24,
			News = 25,
			HowTo = 26,
			Education = 27,
			Science = 28,
			Movies = 30,
			Anime = 31,
			Action = 32,
			Classics = 33,
			ComedyAlt = 34

		}
		public static Video constructVideo(string Title, string Description, string[] tags, Categories category,string PrivacyStatus)
		{
			var video = new Video();
			video.Snippet = new VideoSnippet();
			video.Snippet.Title = Title;
			video.Snippet.Description = Description;
			video.Snippet.Tags = tags;
			video.Snippet.CategoryId = ((int)category).ToString(); // See https://developers.google.com/youtube/v3/docs/videoCategories/list ---> Find ich nicht
			video.Status = new VideoStatus();
			video.Status.PrivacyStatus = PrivacyStatus; // or "private" or "public"
			return video;
		}
		
	}
}
