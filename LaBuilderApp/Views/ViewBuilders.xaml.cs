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

			if (!CrossAppInfo.Current.DisplayName.Equals ("XamarinFormsPreviewer"))
				lvBuilder.ItemsSource = Builder.All;

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
						Builder.LoadData (result);
						Builder.PopulateData ();
					} else {
						Tools.Trace ($"DataRefresh ERROR {x.FileName}: {result}");
					}
				};
				DataServer.AddToDo (builders);

				DataServer.QueueEmpty += () => {
					lvBuilder.EndRefresh ();
					DataServer.QueueEmpty = null;
				};
				DataServer.Launch ();

			};

		}

		private async void ChooseIsDone ()
		{
			Global.SelectedBuilder = lvBuilder.SelectedItem as Builder;
			lvBuilder.SelectedItem = null;
			await Navigation.PushModalAsync (new PageBuilder (), true);
		}

	}
}