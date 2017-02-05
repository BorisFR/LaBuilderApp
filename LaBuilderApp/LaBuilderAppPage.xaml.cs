using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class LaBuilderAppPage : ContentPage
	{
		public LaBuilderAppPage ()
		{
			InitializeComponent ();
			LoadInfo ();
		}

		private void LoadInfo ()
		{
			var model = CrossDeviceInfo.Current.Model;
			var platform = CrossDeviceInfo.Current.Platform;
			var version = CrossDeviceInfo.Current.Version;
			System.Diagnostics.Debug.WriteLine ($"Model: {model}");         // iPhone	/ Xamarin Android Player ('Phone' version)
			System.Diagnostics.Debug.WriteLine ($"Platform: {platform}");   // iOS		// Android
			System.Diagnostics.Debug.WriteLine ($"Version: {version}");     // 10.2		// 5.1
		}

	}
}