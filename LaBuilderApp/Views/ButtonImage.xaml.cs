using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class ButtonImage : ContentView
	{
		public event TriggerObject Clicked;

		public ImageSource ImageSource {
			get { return theImage.Source; }
			set { theImage.Source = value; }
		}

		public string Text { get { return theText.Text; } set { theText.Text = value; } }

		public ButtonImage ()
		{
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				if (Clicked != null)
					Clicked (this, null);
			};
			theLayout.GestureRecognizers.Add (tapGestureRecognizer);

		}

	}
}