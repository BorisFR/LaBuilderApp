using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

			theList.ItemSelected += delegate (object sender, SelectedItemChangedEventArgs e) {
				if (e.SelectedItem == null)
					return;
				Menu m = e.SelectedItem as Menu;
				if (m.Page == MyPage.None) {
					theList.SelectedItem = null;
					return;
				}
				Global.GotoPage (m);
				var ignore = Tools.DelayedGCAsync ();

				try {
					Global.MainAppPage.IsPresented = false;
				} catch (Exception) {
				}
			};
			Tools.Trace ("MenuPage done.");
		}

		public void UnselectMenu ()
		{
			theList.SelectedItem = null;
		}

		public void Goto (MyPage page)
		{
			foreach (MenuGroup mg in MenuManager.All) {
				foreach (Menu m in mg) {
					if (m.Page == page) {
						Global.GotoPage (m);
						var ignore = Tools.DelayedGCAsync ();
						UnselectMenu ();
					}
				}
			}
		}

	}
}