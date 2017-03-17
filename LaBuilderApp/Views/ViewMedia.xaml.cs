using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewMedia : ContentView
	{
		private bool gotoHome = true;

		public ViewMedia ()
		{
			InitializeComponent ();
			Device.BeginInvokeOnMainThread (() => {
				btReload.IsVisible = false;
			});

			theWebview.Navigating += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					if (gotoHome) {
						btReload.IsVisible = false;
						gotoHome = false;
					} else {
						btReload.IsVisible = true;
					}
				});
			};

			btReload.Clicked += (sender, e) => {
				gotoHome = true;
				Device.BeginInvokeOnMainThread (() => {
					theWebview.Source = $"{Global.AppUrl}medias.php";
				});
			};
		}

		~ViewMedia ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

	}
}