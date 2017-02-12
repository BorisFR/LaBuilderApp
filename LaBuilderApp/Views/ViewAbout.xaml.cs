using System;
using System.Collections.Generic;
using Plugin.AppInfo;
using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewAbout : ContentView
	{
		public ViewAbout ()
		{
			InitializeComponent ();
			theWebview.Source = "http://www.r2builders.fr/disclaimer.php";
			LoadInfo ();

		}

		private void LoadInfo ()
		{
			lDevice.Text = $"{CrossDeviceInfo.Current.Model} : {CrossDeviceInfo.Current.Platform} {CrossDeviceInfo.Current.Version}";
			Tools.Trace ("* Device info:");
			Tools.Trace ($"Model: {CrossDeviceInfo.Current.Model}");         // iPhone	/ Xamarin Android Player ('Phone' version)
			Tools.Trace ($"Platform: {CrossDeviceInfo.Current.Platform}");   // iOS		// Android
			Tools.Trace ($"Version: {CrossDeviceInfo.Current.Version}");     // 10.2		// 5.1
			lApp.Text = $"{CrossAppInfo.Current.PackageName} : {CrossAppInfo.Current.DisplayName} - {CrossAppInfo.Current.Version}";
			Tools.Trace ("* App info:");
			Tools.Trace ($"Display: {CrossAppInfo.Current.DisplayName}");    // LaBuilderApp
			Tools.Trace ($"Package: {CrossAppInfo.Current.PackageName}");    // com.boris.labuilderapp
			Tools.Trace ($"Version: {CrossAppInfo.Current.Version}");        // 1.0
		}

	}
}