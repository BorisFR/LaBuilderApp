using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using Plugin.Settings;

namespace LaBuilderApp
{
	public partial class ViewConnect : ContentView
	{
		public ViewConnect ()
		{
			InitializeComponent ();

			lError.Text = string.Empty;

			btLogin.Clicked += (sender, e) => {
				if (eLogin.Text == null) return;
				if (ePassword.Text == null) return;
				Global.CurrentLogin = eLogin.Text.Trim ();
				if (Global.CurrentLogin.Length > 0) {
					CrossSettings.Current.AddOrUpdateValue<string> ("userlogin", Global.CurrentLogin);
					Global.CurrentPassword = ePassword.Text.Trim ();
					if (Global.CurrentPassword.Length > 0) {
						CrossSettings.Current.AddOrUpdateValue<string> ("userpassword", Global.CurrentPassword);
						CrossSettings.Current.AddOrUpdateValue<string> ("usertoken", string.Empty);
						Device.BeginInvokeOnMainThread (() => {
							toCheck.IsVisible = true;
							toLogin.IsVisible = false;
							Exhibition.ClearData ();
							lError.Text = string.Empty;
						});

						IDataServer login = new IDataServer ("login");
						login.IgnoreLocalData = true;
						login.DataRefresh += (obj, status, result) => {
							IDataServer x = obj as IDataServer;
							if (status) {
								Tools.Trace ($"DataRefresh {x.FileName}: {result}");
								// extraire le token
								try {
									Login l = JsonConvert.DeserializeObject<Login> (result);
									Global.CurrentBuilderId = l.UserId;
									Global.CurrentToken = l.Token;
									CrossSettings.Current.AddOrUpdateValue<string> ("usertoken", Global.CurrentToken);
									Global.IsConnected = true;
									RefreshScreen ();
								} catch (Exception err) {
									lError.Text = err.Message;
									Global.IsConnected = false;
								}
							} else {
								Tools.Trace ($"DataRefresh ERROR {x.FileName}: {result}");
								lError.Text = "La connexion a échouée.";
								Global.IsConnected = false;
							}
						};
						DataServer.AddToDo (login);

						DataServer.QueueEmpty += () => {
							RefreshScreen ();
							DataServer.QueueEmpty = null;
						};
						DataServer.Launch ();

					}
				}
			};

			btOut.Clicked += (sender, e) => {
				CrossSettings.Current.AddOrUpdateValue<string> ("userpassword", string.Empty);
				Global.CurrentToken = string.Empty;
				Global.CurrentBuilderId = 0;
				Exhibition.ClearData ();
				lError.Text = string.Empty;
				RefreshScreen ();
			};

			RefreshScreen ();
		}

		private void RefreshScreen ()
		{
			Device.BeginInvokeOnMainThread (() => {
				toCheck.IsVisible = false;
				if (Global.CurrentBuilderId > 0) {
					toLogin.IsVisible = false;
					toOut.IsVisible = true;
					lCompte.Text = $"Builder : {Builder.GetById (Global.CurrentBuilderId).Username}";
					Global.IsConnected = true;
				} else {
					toLogin.IsVisible = true;
					toOut.IsVisible = false;
					Global.IsConnected = false;
				}
				eLogin.Text = Global.CurrentLogin;
			});
		}

	}
}