using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace LaBuilderApp
{
	public partial class PageBuilder : ContentPage
	{
		private int saveIdBuilder = 0;
		private string saveIdThing = string.Empty;
		private string saveIdEvent = string.Empty;

		public PageBuilder ()
		{
			try {
				Global.ComingFromBuilder = Global.SelectedBuilder.UserId;
				saveIdBuilder = Global.SelectedBuilder.UserId;
				saveIdThing = Global.ComingFromThing;
				saveIdEvent = Global.ComingFromEvent;
			} catch (Exception) { }
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Global.ComingFromBuilder = 0;
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);

			this.BindingContext = Global.SelectedBuilder;
			/*
			lvExhibition.ItemSelected += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					if (lvExhibition.SelectedItem == null) return;
					ChooseEvent ();
				});
			};
			*/

			// we're removing the scroll bar of the listview by settings it's size
			//lvExhibition.HeightRequest = 50 * Global.SelectedBuilder.EventsDetail.Count + 20 * Global.SelectedBuilder.Events.Count;

			//lvExhibition.ChildAdded += (sender, e) => {
			//	lvExhibition.HeightRequest = lvExhibition.HeightRequest + (sender as View).Height;
			//};
		}


		~PageBuilder ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

		/*
		private void ChooseEvent ()
		{
			if (Global.ComingFromEvent == ((Exhibition)lvExhibition.SelectedItem).Id) {
				Global.ComingFromBuilder = 0;
				Navigation.PopModalAsync ();
				return;
			}
			Global.SelectedExhibition = lvExhibition.SelectedItem as Exhibition;
			Device.BeginInvokeOnMainThread (() => {
				lvExhibition.SelectedItem = null;
			});
			Navigation.PushModalAsync (new PageAgenda (), true);
		}
		*/
		void ButtonClicked (object sender, EventArgs e)
		{
			Button button = sender as Button;
			string param = (string)button.CommandParameter;
			if (Global.ComingFromThing == param) {
				Global.ComingFromBuilder = 0;
				Navigation.PopModalAsync ();
				return;
			}
			Global.SelectedThing = Thing.GetById (param);
			Navigation.PushModalAsync (new PageThing (), true);
		}

		protected override void OnAppearing ()
		{
			Global.ComingFromBuilder = saveIdBuilder;
			if (saveIdBuilder > 0) Global.SelectedBuilder = Builder.GetById (saveIdBuilder);
			Global.ComingFromThing = saveIdThing;
			if (saveIdThing.Length > 0) Global.SelectedThing = Thing.GetById (saveIdThing);
			Global.ComingFromEvent = saveIdEvent;
			if (saveIdEvent.Length > 0) Global.SelectedExhibition = Exhibition.GetById (saveIdEvent);

			if (Global.SelectedBuilder != null && Global.SelectedBuilder.AllPicturesCount > 0) {
				theCarousel.HeightRequest = Global.ScreenSize.GetHeight ();
				if (Global.SelectedBuilder.AllPicturesCount > 1)
					theCarouselIndicators.IsVisible = true;
				else
					theCarouselIndicators.IsVisible = false;
				Tools.Trace ($"Screen height: {Global.ScreenSize.GetHeight ()}, width: {Global.ScreenSize.GetWidth ()}");
			} else {
				theCarousel.HeightRequest = 1;
				theCarouselIndicators.IsVisible = false;
			}
			base.OnAppearing ();
		}

	}
}