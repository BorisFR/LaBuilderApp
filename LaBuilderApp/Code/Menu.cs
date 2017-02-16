using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using Plugin.AppInfo;
using FFImageLoading.Svg.Forms;

namespace LaBuilderApp
{
	public class Menu : INotifyPropertyChanged
	{
		public MyPage Page { get; set; }
		public string Title { get; set; }

		private string detail;
		public string Detail {
			get { return detail; }
			set {
				if (value.Equals (detail))
					return;
				detail = value;
				OnPropertyChanged ();
			}
		}

		private string icon;
		public string Icon {
			get { return icon; }
			set {
				if (value.Equals (icon))
					return;
				icon = value;
				OnPropertyChanged ();
				OnPropertyChanged ("Image");
			}
		}

		public ImageSource Image {
			get {
				if (Icon.Length == 0) {
					return null;
				}
				if (Device.OS == TargetPlatform.Windows)
					return ImageSource.FromResource (string.Format ("LaBuilderApp.Win.Images.menu_{0}.png", Icon));
				if (CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer"))
					return ImageSource.FromResource (string.Format ("XamarinFormsPreviewer.Images.menu_{0}.png", Icon));
				return ImageSource.FromResource (string.Format ("LaBuilderApp.Images.menu_{0}.png", Icon));
			}
		}

		public FFImageLoading.Work.ImageSource ImageSVG {
			get {
				if (Icon.Length == 0) {
					//return null;
				}


				return SvgImageSource.FromResource (string.Format ("LaBuilderApp.Images.menu_{0}.svg", Icon));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) {
				handler (this, new PropertyChangedEventArgs (propertyName));
			}
		}

	}
}