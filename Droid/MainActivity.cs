using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using EstimoteSdk;
using System.Collections.Generic;

namespace LaBuilderApp.Droid
{
	// [Activity (Label = "LaBuilderApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	[Activity (Label = "LaBuilderApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, BeaconManager.IServiceReadyCallback
	{

		//public static Context TheContext = null;
		public static Beacons TheBeacons = null;
		BeaconManager beaconManager;
		Region region;


		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate (bundle);

			//TheContext = this;

			global::Xamarin.Forms.Forms.Init (this, bundle);


			var metrics = Resources.DisplayMetrics;
			var widthInDp = ConvertPixelsToDp (metrics.WidthPixels);
			var heightInDp = ConvertPixelsToDp (metrics.HeightPixels);
			ScreenSize.ScreenWidth = widthInDp;
			ScreenSize.ScreenHeight = heightInDp;


			try {
				beaconManager = new BeaconManager (this); // (MainActivity.TheContext);
				region = new Region (Global.iBeaconRegion, Global.iBeaconUUID);

				beaconManager.EnteredRegion += (sender, e) => {
					if (TheBeacons != null) TheBeacons.SendMsg ("StartRanging");
					beaconManager.StartRanging (region);
				};
				beaconManager.ExitedRegion += (sender, e) => {
					if (TheBeacons != null) TheBeacons.SendMsg ("StopRanging");
					beaconManager.StopRanging (region);
				};
				beaconManager.Ranging += (sender, e) => {
					List<OneBeacon> thelist = new List<OneBeacon> ();
					foreach (Beacon b in e.Beacons) {
						string major = b.Major.ToString ();
						string minor = b.Minor.ToString ();
						string rssi = b.Rssi.ToString ();
						thelist.Add (new OneBeacon () { Major = major, Minor = minor, Rssi = rssi, Description = "" });
					}
					TheBeacons.GotBeacons (thelist);
				};
			} catch (Exception err) {
				if (TheBeacons != null) TheBeacons.SendMsg ($"Beacon Init error: {err.Message}");
			}


			LoadApplication (new App ());
		}

		public override void OnRequestPermissionsResult (int requestCode, string [] permissions, Permission [] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult (requestCode, permissions, grantResults);
		}


		private int ConvertPixelsToDp (float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}

		void BeaconManager.IServiceReadyCallback.OnServiceReady ()
		{
			if (TheBeacons != null) TheBeacons.SendMsg ("StartMonitoring");
			beaconManager.StartMonitoring (region);
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			if (TheBeacons != null) TheBeacons.SendMsg ("Connect");
			beaconManager.Connect (this);
		}

		protected override void OnDestroy ()
		{
			if (TheBeacons != null) TheBeacons.SendMsg ("Disconnect");
			beaconManager.StopRanging (region);
			beaconManager.StopMonitoring (region);
			beaconManager.Disconnect ();
			base.OnDestroy ();
		}
	}

}