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

		}
	}
}
