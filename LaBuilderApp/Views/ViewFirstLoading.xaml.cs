﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Plugin.Settings;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewFirstLoading : ContentView
	{
		private int step = 0;
		private bool isReady = false;

		public ViewFirstLoading ()
		{
			InitializeComponent ();

			AddText ("Bienvenue");

		}

		~ViewFirstLoading ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

		private void AddText (string text)
		{
			AppearingText t = new AppearingText (text, Color.FromHex ("#5AA9D3"), TextAnimation.AppearAndStop, 8);
			theLayout.Children.Add (t);
			t.AppearDone += async () => {
				step++;
				switch (step) {
				case 1:
					AddText ("Loading data...");
					break;
				case 2:
					Device.StartTimer (new TimeSpan (0, 0, 0, 0, 200), StartLoading);
					break;
				default:
					Tools.Trace ($"Step: {step}");
					if (isReady) {
						await System.Threading.Tasks.Task.Yield ();
						Device.StartTimer (new TimeSpan (0, 0, 0, 0, 600), ChangeScreen);
					}
					break;
				}
			};
		}

		private bool StartLoading ()
		{
			LoadAll ();
			return false;
		}

		private bool ChangeScreen ()
		{
			isReady = false;
			Global.GotoPage (MenuManager.All [0] [0]);
			return false;
		}

		private void LoadAll ()
		{
			if (Global.CurrentToken.Length > 0) {
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
						} catch (Exception err) {
							Tools.Trace ($"DataRefresh ERROR {x.FileName}: {err.Message}");
							Global.IsConnected = false;
						}
					} else {
						Tools.Trace ($"DataRefresh ERROR {x.FileName}: {result}");
						Global.IsConnected = false;
					}
				};
				DataServer.AddToDo (login);
			} else {
				Tools.Trace ("No token");
			}

			IDataServer country = new IDataServer ("country");
			//events.IgnoreLocalData = true;
			country.StartWorking += () => {
				AddText ("Loading country");
			};
			country.DataRefresh += (sender, status, result) => {
				IDataServer x = sender as IDataServer;
				if (status) {
					AddText ("Country done");
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					Country.LoadData (result);
					Country.PopulateData ();
				} else {
					AddText ($"{x.FileName} error");
				}
			};
			DataServer.AddToDo (country);


			IDataServer events = new IDataServer ("events");
			//events.IgnoreLocalData = true;
			events.StartWorking += () => {
				AddText ("Loading events");
			};
			events.DataRefresh += (sender, status, result) => {
				IDataServer x = sender as IDataServer;
				if (status) {
					AddText ("Events done");
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					Device.BeginInvokeOnMainThread (() => {
						Exhibition.LoadData (result);
						Exhibition.PopulateData ();
					});
				} else {
					AddText ($"{x.FileName} error");
				}
			};
			DataServer.AddToDo (events);



			IDataServer thingstype = new IDataServer ("thingstype");
			//events.IgnoreLocalData = true;
			thingstype.StartWorking += () => {
				AddText ("Loading things type");
			};
			thingstype.DataRefresh += (sender, status, result) => {
				IDataServer x = sender as IDataServer;
				if (status) {
					AddText ("Things type done");
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					ThingsType.LoadData (result);
					ThingsType.PopulateData ();
				} else {
					AddText ($"{x.FileName} error");
				}
			};
			DataServer.AddToDo (thingstype);


			IDataServer things = new IDataServer ("things");
			//events.IgnoreLocalData = true;
			things.StartWorking += () => {
				AddText ("Loading things");
			};
			things.DataRefresh += (sender, status, result) => {
				IDataServer x = sender as IDataServer;
				if (status) {
					AddText ("Things done");
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					Device.BeginInvokeOnMainThread (() => {
						Thing.LoadData (result);
						Thing.PopulateData ();
					});
				} else {
					AddText ($"{x.FileName} error");
				}
			};
			DataServer.AddToDo (things);



			IDataServer cards = new IDataServer ("cards");
			cards.StartWorking += () => {
				AddText ("Loading cards");
			};
			cards.DataRefresh += (sender, status, result) => {
				IDataServer x = sender as IDataServer;
				if (status) {
					AddText ("Cards done");
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					Device.BeginInvokeOnMainThread (() => {
						Cards.LoadData (result);
						Cards.PopulateData ();
					});
				} else {
					AddText ($"{x.FileName} error");
				}
			};
			DataServer.AddToDo (cards);



			IDataServer builders = new IDataServer ("builders");
			//events.IgnoreLocalData = true;
			builders.StartWorking += () => {
				AddText ("Loading builders");
			};
			builders.DataRefresh += (sender, status, result) => {
				IDataServer x = sender as IDataServer;
				if (status) {
					AddText ("Builders done");
					Tools.Trace ($"DataRefresh {x.FileName}: {result}");
					Device.BeginInvokeOnMainThread (() => {
						Builder.LoadData (result);
						Builder.PopulateData ();
					});
				} else {
					AddText ($"{x.FileName} error");
				}
			};
			DataServer.AddToDo (builders);

			DataServer.QueueEmpty += () => {
				AddText ("App is ready");
				isReady = true;
				DataServer.QueueEmpty = null;
				var ignore = Tools.DelayedGCAsync ();
			};
			DataServer.Launch ();

		}

	}
}