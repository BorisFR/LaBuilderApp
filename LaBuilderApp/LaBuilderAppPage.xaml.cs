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
			Tools.Trace ("* Device info:");
			Tools.Trace ($"Model: {CrossDeviceInfo.Current.Model}");         // iPhone	/ Xamarin Android Player ('Phone' version)
			Tools.Trace ($"Platform: {CrossDeviceInfo.Current.Platform}");   // iOS		// Android
			Tools.Trace ($"Version: {CrossDeviceInfo.Current.Version}");     // 10.2		// 5.1
			Tools.Trace ("* App info:");
			Tools.Trace ($"Display: {CrossAppInfo.Current.DisplayName}");    // LaBuilderApp
			Tools.Trace ($"Package: {CrossAppInfo.Current.PackageName}");    // com.boris.labuilderapp
			Tools.Trace ($"Version: {CrossAppInfo.Current.Version}");        // 1.0
		}

	}
}