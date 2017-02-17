using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using FFImageLoading.Forms.Droid;

namespace LaBuilderApp.Droid
{
	// [Activity (Label = "LaBuilderApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	[Activity (Label = "LaBuilderApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			CachedImageRenderer.Init ();

			LoadApplication (new App ());
		}

		public override void OnRequestPermissionsResult (int requestCode, string [] permissions, Permission [] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult (requestCode, permissions, grantResults);
		}

	}
}