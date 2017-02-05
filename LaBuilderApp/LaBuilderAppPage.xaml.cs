using Plugin.AppInfo;
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
			System.Diagnostics.Debug.WriteLine ("* Device info:");
			System.Diagnostics.Debug.WriteLine ($"Model: {CrossDeviceInfo.Current.Model}");         // iPhone	/ Xamarin Android Player ('Phone' version)
			System.Diagnostics.Debug.WriteLine ($"Platform: {CrossDeviceInfo.Current.Platform}");   // iOS		// Android
			System.Diagnostics.Debug.WriteLine ($"Version: {CrossDeviceInfo.Current.Version}");     // 10.2		// 5.1
			System.Diagnostics.Debug.WriteLine ("* App info:");
			System.Diagnostics.Debug.WriteLine ($"Display: {CrossAppInfo.Current.DisplayName}");    // LaBuilderApp
			System.Diagnostics.Debug.WriteLine ($"Package: {CrossAppInfo.Current.PackageName}");    // com.boris.labuilderapp
			System.Diagnostics.Debug.WriteLine ($"Version: {CrossAppInfo.Current.Version}");        // 1.0
		}

	}
}