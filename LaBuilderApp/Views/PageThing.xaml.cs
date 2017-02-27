using System;
using System.Collections.Generic;
using Plugin.DeviceInfo;
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
			try {
				Global.ComingFromThing = Global.SelectedThing.Id;
				saveIdBuilder = Global.ComingFromBuilder;
				saveIdEvent = Global.ComingFromEvent;
				saveIdThing = Global.SelectedThing.Id;
			} catch (Exception) { }
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

		private int _position;
		public int Position { get { return _position; } set { _position = value; OnPropertyChanged (); } }

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
			if (Global.SelectedThing.PictureList != null && Global.SelectedThing.PictureList.Length > 0) {
				theCarousel.HeightRequest = Global.ScreenSize.GetHeight ();
				Tools.Trace ($"Screen height: {Global.ScreenSize.GetHeight ()}, width: {Global.ScreenSize.GetWidth ()}");
			} else
				theCarousel.HeightRequest = 1;
			base.OnAppearing ();
		}

	}
}