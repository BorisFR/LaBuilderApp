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