using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using MahApps.Metro;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Threading;
using System.IO;
using MahApps.Metro.Controls;

namespace Csharp_Youtube_Uploader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			

		}
		private async Task Upload(string Title, string Description, string[] tags,video_constructor.Categories category,string PrivacyStatus,string path)
		{
			var credential = await Google_auth.requestUserCredentialUpload();
			var youtuberequest = Youtube_request.getYoutubeService(credential);
			var video = video_constructor.constructVideo(Title,Description,tags,category,PrivacyStatus);
			var filePath = path;
			using (var file = new FileStream(filePath, FileMode.Open))
			{
			var uploadRequest = youtuberequest.Videos.Insert(video, "snippet,status", file, "video/*");
			uploadRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
			uploadRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

			await uploadRequest.UploadAsync();
		}

		}

		private void videosInsertRequest_ResponseReceived(Video obj)
		{
			throw new NotImplementedException();
		}

		private void videosInsertRequest_ProgressChanged(IUploadProgress obj)
		{
			throw new NotImplementedException();
		}

		private void LanguageButton_Click(object sender, RoutedEventArgs e)
		{
			(sender as Button).ContextMenu.IsEnabled = true;
			(sender as Button).ContextMenu.PlacementTarget = (sender as Button);
			(sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
			(sender as Button).ContextMenu.IsOpen = true;
		}




		
	}
}
