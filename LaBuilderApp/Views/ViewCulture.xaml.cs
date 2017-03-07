using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewCulture : ContentView
	{
		public ViewCulture ()
		{
			InitializeComponent ();

			btAlphabet.Clicked += (sender, e) => {
				Global.MenuPage.UnselectMenu ();
				Menu m = new Menu ();
				m.Page = MyPage.Alphabet;
				m.Title = "Alphabet Aurebesh";
				Global.GotoPage (m);
			};

			btGameSweeper.Clicked += (sender, e) => {
				Global.MenuPage.UnselectMenu ();
				Menu m = new Menu ();
				m.Page = MyPage.GameR2Finder;
				m.Title = "R2-Finder";
				Global.GotoPage (m);
			};

			btGameRuzzle.Clicked += (sender, e) => {
				Global.MenuPage.UnselectMenu ();
				Menu m = new Menu ();
				m.Page = MyPage.GameRuzzle;
				m.Title = "R2-Puzzle";
				Global.GotoPage (m);
			};
		}

	}
}