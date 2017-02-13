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

		}

		private async void ChooseIsDone ()
		{
			Global.SelectedBuilder = lvBuilder.SelectedItem as Builder;
			lvBuilder.SelectedItem = null;
			await Navigation.PushModalAsync (new PageBuilder (), true);
		}

	}
}