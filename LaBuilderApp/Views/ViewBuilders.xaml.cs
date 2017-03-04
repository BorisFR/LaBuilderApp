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
					}
				};
				DataServer.AddToDo (builders);

				DataServer.QueueEmpty += () => {
					Device.BeginInvokeOnMainThread (() => {
						lvBuilder.EndRefresh ();
					});
					DataServer.QueueEmpty = null;
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