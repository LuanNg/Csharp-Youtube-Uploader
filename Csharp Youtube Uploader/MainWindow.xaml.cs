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
using Microsoft.Win32;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
			updateProfileLists();
			// Uploads = readCachedUploads();	//NOT FINAL
		}

		private async void Upload(UploadParameters P)
		{
			var credential = await Google_auth.requestUserCredentialUpload(P.ProfileName);
			var youtuberequest = Youtube_request.getYoutubeService(credential);
			var video = video_constructor.constructVideo(P.Title,P.Description,P.tags,P.category,P.PrivacyStatus);
			var filePath = P.path;
			using (var file = new FileStream(filePath, FileMode.Open))
			{
				filesize = file.Length;
				var uploadRequest = youtuberequest.Videos.Insert(video, "snippet,status", file, "video/*");
				MessageBox.Show(youtuberequest.Serializer.Serialize(uploadRequest));
				uploadRequest.ProgressChanged += new Action<Google.Apis.Upload.IUploadProgress>((p) => ProgressHandler(p, uploadRequest));
				uploadRequest.ResponseReceived += uploadRequest_ResponseReceived;
				await uploadRequest.ResumeAsync();
			}
		}

		private async void ResumeUpload()
		{
			
		}	//Creates another Upload instead of Resuming

		private void ProgressHandler(IUploadProgress obj, VideosResource.InsertMediaUpload video)
		{
			if (obj.Status == UploadStatus.Failed)
			{
				MessageBox.Show(obj.Exception.Message);
				foreach (var help in obj.Exception.Data.Values)
				{
					MessageBox.Show(help.ToString());
				}
			}
			if (obj.Status == UploadStatus.Uploading)
			{
				Dispatcher.BeginInvoke(
				new Action(() =>
				{
					for (int i = 0; i < UploadQueue.Items.Count; i++)
					{
						System.Windows.Controls.Border UploadEntry = UploadQueue.Items.GetItemAt(i) as System.Windows.Controls.Border;
						if (UploadEntry.FindChild<TextBlock>("Stats").Text.Contains(video.Body.Snippet.Title))
						{
							UploadEntry.FindChild<ProgressBar>("Progress").Value = obj.BytesSent / filesize;
							string[] Stats = UploadEntry.FindChild<TextBlock>("Stats").Text.Split('\n');
							DateTime StartTime = DateTime.Parse(Stats[2].Substring(12));
							TimeSpan ElapsedTime = DateTime.Now - StartTime;
							TimeSpan RemainingTime = TimeSpan.FromTicks((long)(ElapsedTime.Ticks * (100 - (obj.BytesSent / filesize) * 100)));
							UploadEntry.FindChild<TextBlock>("Stats").Text = Stats[0] + "\n" + Math.Round(obj.BytesSent / filesize, 3) + "%\n" + Stats[2] + "\nFinished in:\t" + RemainingTime.ToString(@"dd\.hh\:mm\:ss");
						}
					}
				})
			);
			}
		}

		private void uploadRequest_ResponseReceived(Video obj)
		{
			Dispatcher.BeginInvoke(
				new Action(() => {
					for (int i=0; i< UploadQueue.Items.Count; i++)
					{	System.Windows.Controls.Border UploadEntry = UploadQueue.Items.GetItemAt(i) as System.Windows.Controls.Border;
						if(UploadEntry.FindChild<TextBlock>("Stats").Text.Contains(obj.Snippet.Title))
						{
							UploadEntry.FindChild<ProgressBar>("Progress").Value = 100;
							string[] Stats = UploadEntry.FindChild<TextBlock>("Stats").Text.Split('\n');
							UploadEntry.FindChild<TextBlock>("Stats").Text = Stats[0] + "\n100%\n" + Stats[2] + "\nFinished";
							UploadEntry.FindChild<TextBlock>("VideoUri").Text = "http://youtube.com/watch?v=" + obj.Id;
							UploadEntry.FindChild<TextBlock>("VideoUri").MouseDown += new MouseButtonEventHandler((s,e) => VideoUri_MouseDown(s,e,obj));
						}
					}
				})
				);
		}

		private void VideoUri_MouseDown(object s, EventArgs e, Video obj)
		{
			System.Diagnostics.Process.Start("http://youtube.com/watch?v=" + obj.Id);
		}

		private void videosInsertRequest_ProgressChanged(IUploadProgress obj)
		{
			if (obj.Status == UploadStatus.Failed)
			{
				MessageBox.Show(obj.Exception.Message);
				foreach (var help in obj.Exception.Data.Values)
				{
					MessageBox.Show(help.ToString());
				}
			}
			if(obj.Status == UploadStatus.Uploading)
			{	Dispatcher.BeginInvoke(
				new Action(() => {
					System.Windows.Controls.Border UploadEntry = UploadQueue.Items.GetItemAt(0) as System.Windows.Controls.Border;
					UploadEntry.FindChild<ProgressBar>("Progress").Value = obj.BytesSent / filesize;
					string []Stats = UploadEntry.FindChild<TextBlock>("Stats").Text.Split('\n');
					DateTime StartTime = DateTime.Parse(Stats[2].Substring(12));
					TimeSpan ElapsedTime = DateTime.Now - StartTime;
					TimeSpan RemainingTime = TimeSpan.FromTicks((long)(ElapsedTime.Ticks * (100 - (obj.BytesSent / filesize)*100)));
					UploadEntry.FindChild<TextBlock>("Stats").Text = Stats[0] + "\n" + Math.Round(obj.BytesSent / filesize, 3) + "%\n" + Stats[2] + "\nFinished in:\t" + (int)RemainingTime.TotalHours + ":" + RemainingTime.ToString(@"mm\:ss");
				})
			);
			}
		}

		private void LanguageButton_Click(object sender, RoutedEventArgs e)
		{
			(sender as Button).ContextMenu.IsEnabled = true;
			(sender as Button).ContextMenu.PlacementTarget = (sender as Button);
			(sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
			(sender as Button).ContextMenu.IsOpen = true;
		}

		private void Upload(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(FileName.Text))
			{
				if (!string.IsNullOrEmpty(ProfileComboBox.SelectionBoxItem.ToString()))
				{
					string Title = VideoTitle.Text;
					UploadQueue.Items.Add(UploadEntry.newUploadEntry(Title));	//Upload Queue Entry
					TabControl.SelectedIndex = 3;								//Switches Tab to Upload Queue
					UploadParameters UploadParameters = new UploadParameters(Title, VideoDescription.Text, VideoTags.Text.Split(','), video_constructor.Categories.Events, GetPrivacyStatus(), FileName.Text, ProfileComboBox.SelectionBoxItem.ToString());
					Upload(UploadParameters);
				}
				else
				{
					MessageBox.Show("Please select a destination Account");
				}
			}
			else
			{
				MessageBox.Show("Please Select a File");
			}
		}

		private void FileButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();

			Nullable<bool> result = dlg.ShowDialog();
			if (result == true)
			{
				string filename = dlg.FileName;
				FileName.Text = filename;

			}
		}

		private string GetPrivacyStatus()
		{
			return PrivacySettings.SelectionBoxItem.ToString().ToLower();
		}

		private void VideoTitle_TextChanged(object sender, TextChangedEventArgs e)
		{
			VideoTitleHeader.Text = "Title: ( " + VideoTitle.Text.Length + " / 100 )";
		}

		private void VideoDescription_TextChanged(object sender, TextChangedEventArgs e)
		{
			VideoDescriptionHeader.Text = "Description: ( " + VideoDescription.Text.Length + " / 5000 )";
		}

		private void VideoTags_TextChanged(object sender, TextChangedEventArgs e)
		{
			//Adjusts Max Length based on # of , since those are not counted
			VideoTags.MaxLength = 500 + VideoTags.Text.Split(',').Length - 1;

			string tags = VideoTags.Text.Replace(",", string.Empty);
			VideoTagsHeader.Text = "Tags: ( " + tags.Length + " / 500 )";
		}

		private void FileName_TextChanged(object sender, TextChangedEventArgs e)
		{
			VideoTitle.Text = System.IO.Path.GetFileNameWithoutExtension(FileName.Text);
		}

		private async void Add_Account(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(ProfileNameBox.Text))
			{
				await Google_auth.requestUserCredentialUpload(ProfileNameBox.Text);
				updateProfileLists();
			}
			else
			{
				MessageBox.Show("Please Enter a Profile Name");
			}
		}

		private void updateProfileLists()
		{
			ProfileComboBox.Items.Clear();
			ProfileList.Items.Clear();

			if(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\C#YTUploader\Youtube.Auth.Store\"))
			{	string[] Files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\C#YTUploader\Youtube.Auth.Store\");
				foreach (string File in Files)
				{
					string rawFileName = System.IO.Path.GetFileName(File);
					string ProfileName = rawFileName.Replace("Google.Apis.Auth.OAuth2.Responses.TokenResponse-", string.Empty);

					ComboBoxItem Profile = new ComboBoxItem();
					Profile.Content = ProfileName;

					ListBoxItem Profile2 = new ListBoxItem();
					Profile2.Content = ProfileName;
		
					ProfileComboBox.Items.Add(Profile);
					ProfileList.Items.Add(Profile2);
				}
			}
		}

		private void ProfileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ProfileList.SelectionChanged -= ProfileList_SelectionChanged;
			RemoveProfile(e.AddedItems[0].ToString().Replace("System.Windows.Controls.ListBoxItem: ", string.Empty));
			ProfileList.SelectionChanged += ProfileList_SelectionChanged;
		}

		private void RemoveProfile(string ProfileName)
		{
			string ConfirmationBox = "Do you really want to delete the Profile\n" + ProfileName + " ?";
			MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(ConfirmationBox, "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\C#YTUploader\Youtube.Auth.Store\Google.Apis.Auth.OAuth2.Responses.TokenResponse-" + ProfileName);
				updateProfileLists();
			}
		}

		/*
		private List<UploadParameters> readCachedUploads()
		{
			string saveFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\C#YTUploader\UploadsTempCache.lst";
			List<UploadParameters> Uploads = new List<UploadParameters>();
			try
			{
				using (Stream stream = File.Open(saveFile, FileMode.Open))
				{
					var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

					Uploads = (List<UploadParameters>)bformatter.Deserialize(stream);
				}
			}
			catch (FileNotFoundException)
			{ }
			foreach (UploadParameters P in Uploads)
			{
				UploadQueue.Items.Add(UploadEntry.newUploadEntry(P.Title));
				//ResumeUpload(P);
			}
			return Uploads;
		}
		*/
	}
}
