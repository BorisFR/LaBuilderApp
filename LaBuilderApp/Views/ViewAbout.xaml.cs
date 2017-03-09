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
			Device.BeginInvokeOnMainThread (() => {
				Global.MenuPage.UnselectMenu ();
				LoadInfo ();
				InfoData ();
			});

			btMentionsLegales.Clicked += (sender, e) => {
				//Navigation.PushModalAsync (new ViewInfo (), true);
				Menu m = new Menu ();
				m.Detail = string.Empty;
				m.Title = "Mentions légales";
				m.Page = MyPage.Legal;
				Global.GotoPage (m);
			};

			btSite.Clicked += (sender, e) => {
				try {
					Device.OpenUri (new Uri (Global.BaseUrl));
				} catch (Exception) {
				}
			};

			btClearCache.Clicked += (sender, e) => {
				Global.ClearDataCache ();
				Menu m = new Menu ();
				m.Detail = string.Empty;
				m.Title = "Chargement des donnees";
				m.Page = MyPage.FirstLoading;
				Global.GotoPage (m);
			};
		}

		~ViewAbout ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

		private async void InfoData ()
		{
			DoFile ("country", "");
			DoFile ("thingstype", "\n");
			DoFile ("builders", "\n");
			DoFile ("things", "\n");
			DoFile ("events", "\n");
		}

		private async void DoFile (string name, string separator)
		{
			IDataServer obj = new IDataServer (name);
			bool exist = await obj.FileIsPresent ();
			DateTime date = obj.FileDate ();
			if (!exist)
				lData.Text = lData.Text + separator + $"File {name}: missing";
			else
				lData.Text = lData.Text + separator + $"File {name}: {date.ToLocalTime ().ToString ("F", Global.CultureFrench)}";
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
			lScreen.Text = $"Screen size: {Global.ScreenSize.GetWidth ().ToString ()}x{Global.ScreenSize.GetHeight ().ToString ()}";
			Tools.Trace ($"Screen size: {Global.ScreenSize.GetWidth ().ToString ()}x{Global.ScreenSize.GetHeight ().ToString ()}");
		}

	}
}