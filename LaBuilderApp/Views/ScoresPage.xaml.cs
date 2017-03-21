using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ScoresPage : ContentPage
	{
		public ScoresPage ()
		{
			InitializeComponent ();
			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgBackground.GestureRecognizers.Add (tapGestureRecognizer);

			PlayerScore.LoadData (Tools.TheWinners);

			lvScore.ItemsSource = PlayerScore.Whole;

			lvScore.ItemSelected += (sender, e) => {
				Navigation.PopModalAsync ();
			};
		}
	}
}