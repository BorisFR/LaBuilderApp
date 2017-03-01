using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
			InitializeComponent ();
			theList.BackgroundColor = this.BackgroundColor;

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Menu m = new Menu ();
				m.Detail = string.Empty;
				m.Title = "A propos de";
				m.Page = MyPage.About;
				Global.GotoPage (m);
				try {
					Global.MainAppPage.IsPresented = false;
				} catch (Exception) {
				}
			};
			imgTitle.GestureRecognizers.Add (tapGestureRecognizer);


			//theList.ItemsSource = Global.Menus.All;
			theList.ItemSelected += delegate (object sender, SelectedItemChangedEventArgs e) {
				if (e.SelectedItem == null)
					return;
				Menu m = e.SelectedItem as Menu;
				if (m.Page == MyPage.None) {
					theList.SelectedItem = null;
					return;
				}
				Global.GotoPage (m);


				//				if (Global.AllBuilders.All.Count == 0) {
				//				} else {
				//					Builder b = Global.AllBuilders.All [Global.Random.Next (Global.AllBuilders.All.Count)];
				//					Global.CurrentBuilder = b;
				//					Global.MainAppPage.Detail = new NavigationPage (new PageBuilder ());
				//					//Navigation.PushAsync (new PageBuilder ());
				//				}

				try {
					Global.MainAppPage.IsPresented = false;
				} catch (Exception) {
				}
				//theList.SelectedItem = null;
			};
			Tools.Trace ("MenuPage done.");
		}

	}
}