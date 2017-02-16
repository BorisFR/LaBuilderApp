using System;
using Plugin.DeviceInfo.Abstractions;
using Xamarin.Forms;

namespace LaBuilderApp
{
	public class StarJedi : Label
	{
		public const string Typeface = "Star Jedi";

		public StarJedi ()
		{
			FontAttributes = FontAttributes.None;
			if (Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform == Platform.Windows)
				FontFamily = @"\Assets\Star Jedi.ttf#Star Jedi";
			else
				FontFamily = Typeface;
		}

	}
}