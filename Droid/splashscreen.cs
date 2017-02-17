
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Permissions;
using Android.Content.PM;


namespace LaBuilderApp.Droid
{
	[Activity (Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class splashscreen : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your application here
			global::Xamarin.Forms.Forms.Init (this, savedInstanceState);

		}

		protected override void OnResume ()
		{
			base.OnResume ();
			StartActivity (new Intent (Application.Context, typeof (MainActivity)));
		}

		public override void OnRequestPermissionsResult (int requestCode, string [] permissions, Permission [] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult (requestCode, permissions, grantResults);
		}

	}
}