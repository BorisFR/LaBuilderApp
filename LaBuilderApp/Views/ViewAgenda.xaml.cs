using System;
using Plugin.AppInfo;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewAgenda : ContentView
	{
		public ViewAgenda ()
		{
			InitializeComponent ();
			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer")) {
				Device.BeginInvokeOnMainThread (() => {
					lvExhibition.ItemsSource = Exhibition.AllGroup;
				});
			}

			if (Exhibition.AllGroup.Count == 0) {
				Device.BeginInvokeOnMainThread (() => {
					lvExhibition.IsRefreshing = true;
					DoRefresh ();
				});
			}

			btPreviousYear.Clicked += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					Exhibition.ChangeToYear (Exhibition.CurrentYear - 1);
					lYear.Text = $"Agenda {Exhibition.CurrentYear.ToString ()}";
				});
			};
			btNextYear.Clicked += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					Exhibition.ChangeToYear (Exhibition.CurrentYear + 1);
					lYear.Text = $"Agenda {Exhibition.CurrentYear.ToString ()}";
				});
			};

			lvExhibition.ItemSelected += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					if (lvExhibition.SelectedItem == null) return;
					ChooseIsDone ();
				});
			};

			lvExhibition.Refreshing += (sender, e) => {
				Device.BeginInvokeOnMainThread (() => {
					DoRefresh ();
				});
			};
		}

		~ViewAgenda ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

		private void DoRefresh ()
		{
			IDataServer events = new IDataServer ("events");
			events.IgnoreLocalData = true;
			events.DataRefresh += (obj, status, result) => {
				IDataServer x = obj as IDataServer;
				if (status) {
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					Device.BeginInvokeOnMainThread (() => {
						Exhibition.LoadData (result);
						Exhibition.PopulateData ();
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

										DataServer.AddToDo (events);

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
			DataServer.AddToDo (events);

			DataServer.QueueEmpty += () => {
				Device.BeginInvokeOnMainThread (() => {
					lvExhibition.EndRefresh ();
				});
				DataServer.QueueEmpty = null;
				var ignore = Tools.DelayedGCAsync ();
			};
			DataServer.Launch ();
		}

		private void ChooseIsDone ()
		{
			Global.SelectedExhibition = lvExhibition.SelectedItem as Exhibition;
			Device.BeginInvokeOnMainThread (() => {
				lvExhibition.SelectedItem = null;
			});
			Navigation.PushModalAsync (new PageAgenda (), true);
		}

	}
}