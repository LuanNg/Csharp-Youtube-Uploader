using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MahApps.Metro.Controls;
using System.Threading;
using Google.Apis.YouTube.v3;

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
			var youtubrequest = Youtube_request.getYoutubeService(credential);
			var video = video_constructor.constructVideo(Title,Description,tags,category,PrivacyStatus);

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
