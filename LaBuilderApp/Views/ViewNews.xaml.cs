using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewNews : ContentView
	{
		public ViewNews ()
		{
			InitializeComponent ();
			lvExhibition.HeightRequest = Exhibition.ComingEventsHeight;
			Tools.Trace ($"ViewNews Setting height: {Exhibition.ComingEventsHeight}");
			LoadInfo ();
		}

		public void LoadInfo ()
		{
			Device.BeginInvokeOnMainThread (() => {
				lInfo.Text = "Hello";
			});
		}

		/*
		protected override void OnSizeAllocated (double width, double height)
		{
			if (lvExhibition != null) {
				lvExhibition.HeightRequest = Exhibition.ComingEventsHeight;
				Tools.Trace ($"Setting height: {Exhibition.ComingEventsHeight}");
			}
			base.OnSizeAllocated (width, height);
		}
		*/
	}
}