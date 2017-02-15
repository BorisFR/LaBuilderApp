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
			btReload.IsVisible = false;

			theWebview.Navigating += (sender, e) => {
				if (gotoHome) {
					btReload.IsVisible = false;
					gotoHome = false;
				} else {
					btReload.IsVisible = true;
				}
			};

			btReload.Clicked += (sender, e) => {
				gotoHome = true;
				theWebview.Source = "http://www.r2builders.fr/boris/medias.php";
			};
		}

	}
}