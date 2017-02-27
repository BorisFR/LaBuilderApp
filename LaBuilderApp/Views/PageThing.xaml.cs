using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class PageThing : ContentPage
	{
		private int saveIdBuilder = 0;
		private string saveIdThing = string.Empty;
		private string saveIdEvent = string.Empty;

		public PageThing ()
		{
			Global.ComingFromThing = Global.SelectedThing.Id;
			saveIdBuilder = Global.ComingFromBuilder;
			saveIdEvent = Global.ComingFromEvent;
			saveIdThing = Global.SelectedThing.Id;
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Global.ComingFromThing = string.Empty;
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);

			this.BindingContext = Global.SelectedThing;
			//theCarousel.HeightRequest = this.Height;
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Button button = sender as Button;
			int param = (int)button.CommandParameter;
			if (Global.ComingFromBuilder == param) {
				Global.ComingFromThing = string.Empty;
				Navigation.PopModalAsync ();
				return;
			}
			Global.SelectedBuilder = Builder.GetById (param);
			Navigation.PushModalAsync (new PageBuilder (), true);
		}

		protected override void OnAppearing ()
		{
			Global.ComingFromBuilder = saveIdBuilder;
			if (saveIdBuilder > 0) Global.SelectedBuilder = Builder.GetById (saveIdBuilder);
			Global.ComingFromThing = saveIdThing;
			if (saveIdThing.Length > 0) Global.SelectedThing = Thing.GetById (saveIdThing);
			Global.ComingFromEvent = saveIdEvent;
			if (saveIdEvent.Length > 0) Global.SelectedExhibition = Exhibition.GetById (saveIdEvent);
			if (Global.SelectedThing.PictureList != null && Global.SelectedThing.PictureList.Length > 0)
				theCarousel.HeightRequest = this.Height;
			else
				theCarousel.HeightRequest = 1;
			base.OnAppearing ();
		}

	}
}