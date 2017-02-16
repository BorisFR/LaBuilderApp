using System;
using Plugin.DeviceInfo.Abstractions;
using Xamarin.Forms;
namespace LaBuilderApp
{
	public class TieWing : Label
	{
		public const string Typeface = "TIE-Wing";

		public TieWing ()
		{
			FontAttributes = FontAttributes.None;
			if (Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform == Platform.Windows)
				FontFamily = @"\Assets\TIE-Wing.ttf#TIE-Wing";
			else
				FontFamily = Typeface;
		}

	}
}