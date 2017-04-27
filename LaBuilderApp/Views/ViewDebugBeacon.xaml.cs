using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewDebugBeacon : ContentView
	{
		public ViewDebugBeacon ()
		{
			InitializeComponent ();

			lvVisible.ItemsSource = BeaconStuff.AllVisible;
			lvFound.ItemsSource = BeaconStuff.AllFound;
			eRssi.Text = BeaconStuff.FOUNDRSSI.ToString ();

			btClearAll.Clicked += (sender, e) => {
				Global.JobBeacon.ClearCache ();
			};

			btRssi.Clicked += (sender, e) => {
				string temp = eRssi.Text.Trim ();
				if (temp.Length == 0) return;
				int res;
				if (int.TryParse (temp, out res))
					BeaconStuff.FOUNDRSSI = res;
			};

		}

	}
}