using System;
using System.Collections.Generic;
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

		private void AddText (string text)
		{
			AppearingText t = new AppearingText (text, Color.FromHex ("#5AA9D3"), TextAnimation.AppearAndStop, 8);
			theLayout.Children.Add (t);
			t.AppearDone += () => {
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
					Exhibition.LoadData (result);
					Exhibition.PopulateData ();
				} else {
					AddText ($"{x.FileName} error");
				}
			};
			DataServer.AddToDo (events);

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
					Builder.LoadData (result);
					Builder.PopulateData ();
				} else {
					AddText ($"{x.FileName} error");
				}
			};
			DataServer.AddToDo (builders);

			DataServer.QueueEmpty += () => {
				AddText ("App is ready");
				isReady = true;
				DataServer.QueueEmpty = null;
			};
			DataServer.Launch ();

		}

	}
}