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

			lTitle.Text = Global.CurrentMenu.Title;
			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgBackground.GestureRecognizers.Add (tapGestureRecognizer);
			lTitle.GestureRecognizers.Add (tapGestureRecognizer);
			lSubTitle.GestureRecognizers.Add (tapGestureRecognizer);

			PlayerScore.PopulateData (Tools.TheWinners);

			lvScore.ItemsSource = PlayerScore.Whole;

			lvScore.ItemSelected += (sender, e) => {
				Navigation.PopModalAsync ();
			};
		}
	}
}