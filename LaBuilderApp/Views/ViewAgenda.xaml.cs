using System;
using System.Collections.Generic;
using Plugin.AppInfo;
using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewAgenda : ContentView
	{
		public ViewAgenda ()
		{
			InitializeComponent ();
			lvExhibition.ItemsSource = Exhibition.All;
			LoadInfo ();

			btPreviousYear.Clicked += (sender, e) => {
				Exhibition.ChangeToYear (Exhibition.CurrentYear - 1);
				lYear.Text = $"Agenda {Exhibition.CurrentYear.ToString ()}";
			};
			btNextYear.Clicked += (sender, e) => {
				Exhibition.ChangeToYear (Exhibition.CurrentYear + 1);
				lYear.Text = $"Agenda {Exhibition.CurrentYear.ToString ()}";
			};
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