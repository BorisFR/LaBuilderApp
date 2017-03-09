using System;
using System.Collections.Generic;
using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewThings : ContentView
	{
		public ViewThings ()
		{
			InitializeComponent ();

			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) {
				Device.BeginInvokeOnMainThread (() => {
					lvThing.ItemsSource = Thing.AllGroup;
				});
			}

			lvThing.ItemSelected += (sender, e) => {
				if (lvThing.SelectedItem == null) return;
				ChooseThing ();
			};

			lvThing.Refreshing += (sender, e) => {
				IDataServer things = new IDataServer ("things");
				things.IgnoreLocalData = true;
				things.DataRefresh += (obj, status, result) => {
					IDataServer x = obj as IDataServer;
					if (status) {
						Tools.Trace ($"DataRefresh {x.FileName}: {result}");
						Device.BeginInvokeOnMainThread (() => {
							Thing.LoadData (result);
							Thing.PopulateData ();
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

											DataServer.AddToDo (things);

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
				DataServer.AddToDo (things);

				DataServer.QueueEmpty += () => {
					Device.BeginInvokeOnMainThread (() => {
						lvThing.EndRefresh ();
					});
					DataServer.QueueEmpty = null;
					var ignore = Tools.DelayedGCAsync ();
				};
				DataServer.Launch ();

			};

		}

		~ViewThings ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

		private void ChooseThing ()
		{
			Global.SelectedThing = lvThing.SelectedItem as Thing;
			Device.BeginInvokeOnMainThread (() => {
				lvThing.SelectedItem = null;
			});
			Navigation.PushModalAsync (new PageThing (), true);
		}

	}
}