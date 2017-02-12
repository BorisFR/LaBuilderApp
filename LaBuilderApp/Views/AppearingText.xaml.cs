using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{

	public enum TextAnimation
	{
		Wait,
		Appear,
		Disappear,
		AppearAurekBesh,
		DisappearAurekBesh,
		AppearAndStop
	}

	public partial class AppearingText : ContentView
	{

		public Trigger AppearDone;

		private string theText = string.Empty;

		public string TheText {
			get { return theText; }
			set {
				theText = value;
				LaunchAnimation ();
			}
		}

		private TextAnimation currentAnimation = TextAnimation.Appear;
		public TextAnimation Animation { get { return currentAnimation; } set { currentAnimation = value; } }

		private Color theColor = Color.FromHex ("5AA9D3");
		private double theSize = 16.0;
		private double theSize2 = 12.0;

		private int currentPos = 0;
		private bool runningAnimation = false;
		private int delayBeforeNextAnimation = 0;
		private int pauseAnimation = 0;
		private int delayBlink = 0;
		private bool cursorIsVisible = true;

		private string newText = string.Empty;
		public void ChangeText (string text)
		{
			newText = text;
			switch (currentAnimation) {
			case TextAnimation.Appear:
				currentAnimation = TextAnimation.Disappear;
				break;
			case TextAnimation.AppearAurekBesh:
				currentAnimation = TextAnimation.DisappearAurekBesh;
				break;
			}
			if (pauseAnimation > 1)
				pauseAnimation = 1;
			if (currentPos < 1)
				currentPos = 1;
		}

		public AppearingText ()
		{
			InitializeComponent ();

			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text)
		{
			InitializeComponent ();

			TheText = text;
			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text, Color textColor)
		{
			InitializeComponent ();

			TheText = text;
			theColor = textColor;
			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text, Color textColor, TextAnimation anim)
		{
			InitializeComponent ();

			TheText = text;
			theColor = textColor;
			currentAnimation = anim;
			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text, Color textColor, TextAnimation anim, double fontSize)
		{
			InitializeComponent ();

			TheText = text;
			theColor = textColor;
			currentAnimation = anim;
			theSize = fontSize;
			theSize2 = fontSize;
			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text, double fontSize, double fontSize2)
		{
			InitializeComponent ();

			TheText = text;
			theSize = fontSize;
			theSize2 = fontSize2;
			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text, Color textColor, double fontSize, double fontSize2)
		{
			InitializeComponent ();

			TheText = text;
			theColor = textColor;
			theSize = fontSize;
			theSize2 = fontSize2;
			LaunchAnimation ();
			StartTimer ();
		}

		private void StartTimer ()
		{
			//Tools.Trace ("Anim starting timer...");
			Device.StartTimer (new TimeSpan (0, 0, 0, 0, 50 + Global.Random.Next (40)), DoAnimation);
			//Tools.Trace ($"Anim start timer OK : {currentAnimation}");
		}

		private void LaunchAnimation ()
		{
			if (runningAnimation) return;
			//runningAnimation = false;
			currentPos = 0;
			try {
				labelAnim.Text = string.Empty;
				labelText.Text = string.Empty;
				labelAnim.TextColor = theColor;
				labelText.TextColor = theColor;
				labelAnim.FontSize = theSize2;
				labelText.FontSize = theSize;
				theBox.BackgroundColor = theColor;
				theBox.MinimumHeightRequest = 12;
				theBox.HeightRequest = 24; //labelText.Height;
			} catch (Exception err) {
				//Tools.Trace ($"Anim-launch: {err.Message}");
			}
			runningAnimation = true;
			//Tools.Trace ($"Anim running OK ; {currentAnimation}");
		}

		private void ChooseRandomAnimation ()
		{
			switch (Global.Random.Next (2)) {
			case 0:
				currentAnimation = TextAnimation.Appear;
				break;
			case 1:
				currentAnimation = TextAnimation.AppearAurekBesh;
				break;
			}
		}

		private void DoPauseAnimation ()
		{
			pauseAnimation = Global.Random.Next (10) * 10;
		}

		private bool DoAnimation ()
		{
			if (!runningAnimation)
				return true;
			if (pauseAnimation > 0) {
				pauseAnimation--;

				if (delayBlink < 1) {
					delayBlink = 3;
					if (cursorIsVisible)
						theBox.BackgroundColor = Color.Transparent;
					else
						theBox.BackgroundColor = theColor;
					cursorIsVisible = !cursorIsVisible;
				} else
					delayBlink--;

				return true;
			}
			if (!cursorIsVisible) {
				theBox.BackgroundColor = theColor;
				cursorIsVisible = !cursorIsVisible;
			}
			try {

				switch (currentAnimation) {

				case TextAnimation.Wait:
					if (delayBeforeNextAnimation < 1) {
						delayBeforeNextAnimation = Global.Random.Next (3) * 10;
						ChooseRandomAnimation ();
					} else {
						delayBeforeNextAnimation--;
						SwapText ();
					}
					break;

				case TextAnimation.Appear:
					//Tools.Trace (".");
					if (currentPos > theText.Length)
						return true;
					currentPos++;
					if (currentPos > 1)
						labelText.Text = theText.Substring (0, currentPos - 1);
					else
						labelText.Text = " ";
					if (currentPos <= theText.Length)
						labelAnim.Text = theText.Substring (currentPos - 1, 1);
					else {
						labelAnim.Text = " ";
						DoPauseAnimation ();
						currentAnimation = TextAnimation.Disappear;
					}
					//Tools.Trace ("..");
					break;

				case TextAnimation.AppearAurekBesh:
					if (currentPos > theText.Length)
						return true;
					currentPos++;
					if (currentPos <= theText.Length)
						labelAnim.Text = theText.Substring (0, currentPos);
					else {
						DoPauseAnimation ();
						currentAnimation = TextAnimation.DisappearAurekBesh;
					}
					break;

				case TextAnimation.Disappear:
					if (currentPos < 1)
						return true;
					currentPos--;
					if (currentPos > 1)
						labelText.Text = theText.Substring (0, currentPos - 1);
					else {
						labelText.Text = " ";
						SwapText ();
						currentAnimation = TextAnimation.Wait;
					}
					break;

				case TextAnimation.DisappearAurekBesh:
					if (currentPos < 1)
						return true;
					currentPos--;
					if (currentPos > 1)
						labelAnim.Text = theText.Substring (0, currentPos - 1);
					else {
						labelAnim.Text = " ";
						SwapText ();
						currentAnimation = TextAnimation.Wait;
					}
					break;


				case TextAnimation.AppearAndStop:
					if (currentPos > theText.Length) {
						theBox.BackgroundColor = Color.Transparent;
						if (AppearDone != null) AppearDone ();
						return false;
					}
					currentPos++;
					if (currentPos > 1)
						labelText.Text = theText.Substring (0, currentPos - 1);
					break;
				}

			} catch (Exception err) {
				//Tools.Trace ($"Anim: {err.Message}");
			}
			return true;
		}

		private void SwapText ()
		{
			if (newText.Length > 0) {
				theText = newText;
				newText = string.Empty;
			}
		}

	}
}