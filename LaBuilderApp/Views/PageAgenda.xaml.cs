using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class PageAgenda : ContentPage
	{
		private int saveIdBuilder = 0;
		private string saveIdThing = string.Empty;
		private string saveIdEvent = string.Empty;

		public PageAgenda ()
		{
			Global.ComingFromEvent = Global.SelectedExhibition.Id;
			saveIdEvent = Global.SelectedExhibition.Id;
			saveIdBuilder = Global.ComingFromBuilder;
			saveIdThing = Global.ComingFromThing;
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Global.ComingFromEvent = string.Empty;
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);

			this.BindingContext = Global.SelectedExhibition;
		}

		~PageAgenda ()
		{
			var ignore = Tools.DelayedGCAsync ();
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
			base.OnAppearing ();
		}

	}
}