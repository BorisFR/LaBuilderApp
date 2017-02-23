using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class PageBuilder : ContentPage
	{
		public PageBuilder ()
		{
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);

			this.BindingContext = Global.SelectedBuilder;
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Button button = sender as Button;
			string param = (string)button.CommandParameter;
			Global.SelectedThing = Thing.GetById (param);
			Navigation.PushModalAsync (new PageThing (), true);
		}

	}
}