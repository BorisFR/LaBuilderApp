using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewConnect : ContentView
	{
		public ViewConnect ()
		{
			InitializeComponent ();

			btLogin.Clicked += (sender, e) => {
				Global.CurrentLogin = eLogin.Text.Trim ();
				if (Global.CurrentLogin.Length > 0) {
					Global.CurrentPassword = ePassword.Text.Trim ();
					if (Global.CurrentPassword.Length > 0) {
						toCheck.IsVisible = true;
						toLogin.IsVisible = false;
						Exhibition.ClearData ();
					}
				}
			};

			btOut.Clicked += (sender, e) => {
				Global.CurrentToken = string.Empty;
				Global.CurrentBuilderId = 0;
				Exhibition.ClearData ();
				RefreshScreen ();
			};
		}

		private void RefreshScreen ()
		{
			toCheck.IsVisible = false;
			if (Global.CurrentBuilderId > 0) {
				toLogin.IsVisible = false;
				toOut.IsVisible = true;
				lCompte.Text = $"Builder connecté : {Builder.GetById (Global.CurrentBuilderId).Username}";

			} else {
				toLogin.IsVisible = true;
				toOut.IsVisible = false;
			}
			eLogin.Text = Global.CurrentLogin;

		}

	}
}