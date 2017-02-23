using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class PageThing : ContentPage
	{
		public PageThing ()
		{
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);


			this.BindingContext = Global.SelectedThing;
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Button button = sender as Button;
			int param = (int)button.CommandParameter;
			Global.SelectedBuilder = Builder.GetById (param);
			Navigation.PushModalAsync (new PageBuilder (), true);
		}

	}
}