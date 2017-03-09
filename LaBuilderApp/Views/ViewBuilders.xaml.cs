using System;
using System.Collections.Generic;
using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewBuilders : ContentView
	{
		public ViewBuilders ()
		{
			InitializeComponent ();

			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) {
				Device.BeginInvokeOnMainThread (() => {
					lvBuilder.ItemsSource = Builder.AllGroup;
				});
			}

			lvBuilder.ItemSelected += (sender, e) => {
				if (lvBuilder.SelectedItem == null) return;
				ChooseIsDone ();
			};

			lvBuilder.Refreshing += (sender, e) => {
				IDataServer builders = new IDataServer ("builders");
				builders.IgnoreLocalData = true;
				builders.DataRefresh += (obj, status, result) => {
					IDataServer x = obj as IDataServer;
					if (status) {
						Tools.Trace ($"DataRefresh {x.FileName}: {result}");
						Device.BeginInvokeOnMainThread (() => {
							Builder.LoadData (result);
							Builder.PopulateData ();
						});
					} else {
						Tools.Trace ($"DataRefresh ERROR {x.FileName}: {result}");


						if (Global.CurrentToken.Length > 0) {
							if (Global.IsFirstTimeError) {
								Global.IsFirstTimeError = false;
								IDataServer login = new IDataServer ("login");
								login.IgnoreLocalData = true;
								login.DataRefresh += (obj2, status2, result2) => {
									IDataServer x2 = obj2 as IDataServer;
									if (status2) {
										Tools.Trace ($"DataRefresh {x2.FileName}: {result2}");
										// extraire le token
										try {
											Login l = Newtonsoft.Json.JsonConvert.DeserializeObject<Login> (result2);
											Global.CurrentBuilderId = l.UserId;
											Global.CurrentToken = l.Token;
											Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string> ("usertoken", Global.CurrentToken);
											Global.IsConnected = true;

											DataServer.AddToDo (builders);

										} catch (Exception err2) {
											Tools.Trace ($"DataRefresh ERROR {x2.FileName}: {err2.Message}");
											Global.IsConnected = false;
										}
									} else {
										Tools.Trace ($"DataRefresh ERROR {x2.FileName}: {result2}");
										Global.IsConnected = false;
									}
								};
								DataServer.AddToDo (login);
							}
						}


					}
				};
				DataServer.AddToDo (builders);

				DataServer.QueueEmpty += () => {
					Device.BeginInvokeOnMainThread (() => {
						lvBuilder.EndRefresh ();
					});
					DataServer.QueueEmpty = null;
					var ignore = Tools.DelayedGCAsync ();
				};
				DataServer.Launch ();

			};

		}

		~ViewBuilders ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

		private void ChooseIsDone ()
		{
			Global.SelectedBuilder = lvBuilder.SelectedItem as Builder;
			Device.BeginInvokeOnMainThread (() => {
				lvBuilder.SelectedItem = null;
			});
			Navigation.PushModalAsync (new PageBuilder (), true);
		}

	}
}