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
				ChooseIsDone ();
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
					}
				};
				DataServer.AddToDo (things);

				DataServer.QueueEmpty += () => {
					Device.BeginInvokeOnMainThread (() => {
						lvThing.EndRefresh ();
					});
					DataServer.QueueEmpty = null;
				};
				DataServer.Launch ();

			};

		}

		private void ChooseIsDone ()
		{
			Global.SelectedThing = lvThing.SelectedItem as Thing;
			Device.BeginInvokeOnMainThread (() => {
				lvThing.SelectedItem = null;
			});
			Navigation.PushModalAsync (new PageThing (), true);
		}

	}
}