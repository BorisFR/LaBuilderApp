using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class PageAgenda : ContentPage
	{
		public PageAgenda ()
		{
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);


			this.BindingContext = Global.SelectedExhibition;
		}

	}
}