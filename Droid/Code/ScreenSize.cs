using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Telephony;
using Android.Provider;
using Android.Util;
using Android.Views;
using Android.Runtime;
using Java.Lang;
using B = Android.OS.Build;
using LaBuilderApp.Droid;

[assembly: Xamarin.Forms.Dependency (typeof (ScreenSize))]

namespace LaBuilderApp.Droid
{
	public class ScreenSize : IScreenSize
	{
		public static int ScreenHeight = 0;
		public static int ScreenWidth = 0;

		private void GetInfo ()
		{
			var windowManager = (IWindowManager)Application
				.Context
				.GetSystemService (Context.WindowService)
				.JavaCast<IWindowManager> ();

			if (B.VERSION.SdkInt >= BuildVersionCodes.Honeycomb) {
				var size = new Point ();
				try {
					windowManager.DefaultDisplay.GetRealSize ((Android.Graphics.Point)size);
					ScreenHeight = size.Y;
					ScreenWidth = size.X;
				} catch (NoSuchMethodError) {
					ScreenHeight = windowManager.DefaultDisplay.Height;
					ScreenWidth = windowManager.DefaultDisplay.Width;
				}
			} else {
				var metrics = new DisplayMetrics ();
				windowManager.DefaultDisplay.GetMetrics (metrics);
				ScreenHeight = metrics.HeightPixels;
				ScreenWidth = metrics.WidthPixels;
			}
		}

		int IScreenSize.GetHeight ()
		{
			if (ScreenHeight == 0) GetInfo ();
			return ScreenHeight;
		}

		int IScreenSize.GetWidth ()
		{
			if (ScreenHeight == 0) GetInfo ();
			return ScreenWidth;
		}
	}
}