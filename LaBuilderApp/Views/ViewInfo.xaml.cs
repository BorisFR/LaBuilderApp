using System;
using System.Collections.Generic;
using Plugin.AppInfo;
using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewInfo : ContentView
	{
		public ViewInfo ()
		{
			InitializeComponent ();
			//Device.BeginInvokeOnMainThread (() => {
			//	theWebview.Source = "http://www.r2builders.fr/disclaimer.php";
			//});
		}

		~ViewInfo ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

	}
}