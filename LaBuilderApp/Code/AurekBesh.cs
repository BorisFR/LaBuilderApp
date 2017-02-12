using System;
using Plugin.DeviceInfo.Abstractions;
using Xamarin.Forms;
namespace LaBuilderApp
{
	public class AurekBesh : Label
	{
		public const string Typeface = "Aurek-Besh";

		public AurekBesh ()
		{
			if (Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform == Platform.Windows)
				FontFamily = @"\Assets\Aurek-Besh.ttf#Aurek-Besh";
			else
				FontFamily = Typeface;    //iOS is happy with this, Android needs a renderer to add ".ttf"
		}

	}
}