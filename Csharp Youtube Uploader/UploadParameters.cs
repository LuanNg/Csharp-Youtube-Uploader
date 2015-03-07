using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_Youtube_Uploader
{
	[Serializable]
	class UploadParameters
	{
		public string Title;
		public string Description;
		public string[] tags;
		public video_constructor.Categories category;
		public string PrivacyStatus;
		public string path;
		public string ProfileName;

		public UploadParameters(string Title, string Description, string[] tags, video_constructor.Categories category, string PrivacyStatus, string path, string ProfileName)
		{
			this.Title = Title;
			this.Description = Description;
			this.tags = tags;
			this.category = category;
			this.PrivacyStatus = PrivacyStatus;
			this.path = path;
			this.ProfileName = ProfileName;
		}
	}
}
