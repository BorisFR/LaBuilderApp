using System;
namespace LaBuilderApp.Droid
{
	public class MyPicture : IMyPicture
	{
		public byte [] DoMerge (byte [] background, byte [] overlay, int x, int y, int width, int height, string legend, int red, int green, int blue)
		{
			// https://forums.xamarin.com/discussion/37647/cross-platform-crop-image-view
			return null;
		}

	}
}