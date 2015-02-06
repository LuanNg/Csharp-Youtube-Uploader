using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Csharp_Youtube_Uploader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		double filesize;
		
		public MainWindow()
		{
			InitializeComponent();
			MessageBox.Show("Hue?");
			MessageBox.Show("Hue");

		}
		private async Task Upload(string Title, string Description, string[] tags,video_constructor.Categories category,string PrivacyStatus,string path)
		{
			var credential = await Google_auth.requestUserCredentialUpload();
			var youtuberequest = Youtube_request.getYoutubeService(credential);
			var video = video_constructor.constructVideo(Title,Description,tags,category,PrivacyStatus);
			var filePath = path;
			MessageBox.Show("Opening file...");
			using (var file = new FileStream(filePath, FileMode.Open))
			{
				filesize = file.Length;
				var uploadRequest = youtuberequest.Videos.Insert(video, "snippet,status", file, "video/*");
				MessageBox.Show(uploadRequest.Body.ToString());
				uploadRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
				uploadRequest.ResponseReceived += videosInsertRequest_ResponseReceived;
				MessageBox.Show("Uploading...");
				await uploadRequest.UploadAsync();
			}

		}

		private void videosInsertRequest_ResponseReceived(Video obj)
		{
			
		}

		private void videosInsertRequest_ProgressChanged(IUploadProgress obj)
		{
			if (obj.Status == UploadStatus.Completed)
			{
				MessageBox.Show("Upload completed");
			}
			else
			{
				if (obj.Status == UploadStatus.Failed)
				{
					MessageBox.Show(obj.Exception.Message);
					foreach (var help in obj.Exception.Data.Values)
					{
						MessageBox.Show(help.ToString());
					}
				}
				MessageBox.Show(obj.BytesSent.ToString());

				System.Windows.Controls.Border test = UploadQueue.Items.GetItemAt(0) as System.Windows.Controls.Border;
				test.FindChild<ProgressBar>("Progress").Value = obj.BytesSent / filesize;
			}
		}

		private void LanguageButton_Click(object sender, RoutedEventArgs e)
		{
			(sender as Button).ContextMenu.IsEnabled = true;
			(sender as Button).ContextMenu.PlacementTarget = (sender as Button);
			(sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
			(sender as Button).ContextMenu.IsOpen = true;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			UploadQueue.Items.Add(UploadEntry.newUploadEntry());
			
		}

		private void Upload(object sender, RoutedEventArgs e)
		{
			Upload("Test Video", "Testing", new string[] { "hue", "huehue" }, video_constructor.Categories.Events, "unlisted", @"C:\\Users\\Fabian\\AppData\\Roaming\\Skype\\My Skype Received Files\\Updatevideo 31.1.15.mkv").Wait();
		}
	}
}
