using System;
namespace LaBuilderApp
{
	public interface IMyPicture
	{
		byte [] DoMerge (byte [] background, byte [] overlay, int x, int y, int width, int height, string legend, int red, int green, int blue);
	}
}