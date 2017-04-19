using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewMyCards : ContentView
	{
		public ViewMyCards ()
		{
			InitializeComponent ();

			theList.ItemsSource = Cards.GetForBuilder (Global.CurrentBuilderId);

			btCreate.Clicked += (sender, e) => {
				Menu m = new Menu ();
				m.Page = MyPage.CropCard;
				m.Title = "Créer une carte";
				m.Detail = "Créer une carte";
				Global.GotoPage (m);
			};
		}

		async void ButtonClicked (object sender, EventArgs e)
		{
			Button button = sender as Button;
			string param = (string)button.CommandParameter;
			bool res = await Global.MainAppPage.DisplayAlert ("Confirmation", "Supprimer cette carte ?", "Supprimer", "Annuler");
			if (res) {
				Tools.Trace ($"Suppression de {param}...");
				Tools.DeleteCard (param);
				Cards.RemoveOne (param);
				Global.MenuPage.Goto (MyPage.ListCards);
			}
		}

	}
}