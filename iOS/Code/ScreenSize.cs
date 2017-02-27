using System;
using LaBuilderApp.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency (typeof (ScreenSize))]

namespace LaBuilderApp.iOS
{
	public class ScreenSize : IScreenSize
	{
		int IScreenSize.GetHeight ()
		{
			return (int)UIScreen.MainScreen.Bounds.Height;
		}

		int IScreenSize.GetWidth ()
		{
			return (int)UIScreen.MainScreen.Bounds.Width;
		}

	}
}