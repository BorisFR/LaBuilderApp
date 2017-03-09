using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewNews : ContentView
	{
		public ViewNews ()
		{
			InitializeComponent ();
			lvExhibition.HeightRequest = Exhibition.ComingEventsHeight;

			lvExhibition.ItemSelected += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					if (lvExhibition.SelectedItem == null) return;
					ChooseIsDone ();
				});
			};

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				try {
					Device.OpenUri (new Uri (Global.BaseUrl));
				} catch (Exception) {
				}
			};
			imgSite.GestureRecognizers.Add (tapGestureRecognizer);


		}

		~ViewNews ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

		private void ChooseIsDone ()
		{
			Global.SelectedExhibition = lvExhibition.SelectedItem as Exhibition;
			Device.BeginInvokeOnMainThread (() => {
				lvExhibition.SelectedItem = null;
			});
			Navigation.PushModalAsync (new PageAgenda (), true);
		}

	}
}