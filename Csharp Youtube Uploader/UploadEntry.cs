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
using System.IO;

namespace Csharp_Youtube_Uploader
{
	class UploadEntry
	{
		public static Border newUploadEntry()
		{
			Grid UploadEntry = new Grid()
			{
				Height = 48,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Top,
			};

			Border Border = new Border()
			{
				Padding = new Thickness(2),
				Child = UploadEntry,
			};

			Image Thumbnail = new Image()
			{
				HorizontalAlignment = HorizontalAlignment.Left,
			};

			BitmapImage logo = new BitmapImage();
			logo.BeginInit();
			logo.StreamSource = new FileStream("NoThumb.png", FileMode.Open, FileAccess.Read);
			logo.EndInit();

			Thumbnail.Source = logo;
			UploadEntry.Children.Add(Thumbnail);

			ProgressBar Progress = new ProgressBar()
			{
				Name = "Progress",
			};
			Thickness ProgressMargin = new Thickness();
			ProgressMargin.Left = 90;
			ProgressMargin.Top = 19;
			ProgressMargin.Right = 180;
			ProgressMargin.Bottom = 19;
			Progress.Margin = ProgressMargin;

			UploadEntry.Children.Add(Progress);

			TextBlock Stats = new TextBlock()
			{
				Name = "Stats",
				Text = "test",
			};
			Thickness StatsMargin = new Thickness();
			StatsMargin.Left = 660;
			Stats.Margin = StatsMargin;

			UploadEntry.Children.Add(Stats);

			return Border;
		}
	}
}
