using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ViewAureBesh : ContentView
	{
		public ViewAureBesh ()
		{
			InitializeComponent ();

			if (LettersAurekBesh.All.Count == 0)
				LettersAurekBesh.PopulateData ();

			theAlphabet.ItemsSource = LettersAurekBesh.All;
		}

		~ViewAureBesh ()
		{
			var ignore = Tools.DelayedGCAsync ();
		}

	}
}