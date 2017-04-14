using System;
using CoreGraphics;
using Foundation;
using LaBuilderApp.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency (typeof (MyPicture))]

namespace LaBuilderApp.iOS
{
	public class MyPicture : IMyPicture
	{

		public byte [] DoMerge (byte [] background, byte [] overlay, int x, int y, int width, int height, string legend, int red, int green, int blue)
		{
			var data1 = NSData.FromArray (background);
			UIImage img1 = UIImage.LoadFromData (data1);
			var data2 = NSData.FromArray (overlay);
			UIImage img2 = UIImage.LoadFromData (data2);

			CGSize newimage = new CGSize (img2.Size.Width, img2.Size.Height);
			var cGBitmapContext = new CGBitmapContext (
				IntPtr.Zero, (int)img2.Size.Width, (int)img2.Size.Height,
				8, (int)(4 * img2.Size.Width),
				CGColorSpace.CreateDeviceRGB (), CGImageAlphaInfo.PremultipliedFirst);
			var rectBackground = new CGRect (x, img2.Size.Height - height - y, width, height);
			var working = new CGRect (0f, 0f, img2.Size.Width, img2.Size.Height);
			cGBitmapContext.DrawImage (rectBackground, img1.CGImage);
			cGBitmapContext.DrawImage (working, img2.CGImage);

			cGBitmapContext.SelectFont ("Arial-BoldMT", 70, CGTextEncoding.MacRoman);
			float startX, endX, textWidth;

			startX = (float)cGBitmapContext.TextPosition.X;
			cGBitmapContext.SetTextDrawingMode (CGTextDrawingMode.Invisible);
			cGBitmapContext.ShowText (legend);
			endX = (float)cGBitmapContext.TextPosition.X;
			textWidth = endX - startX;

			/*nfloat fRed;
			nfloat fGreen;
			nfloat fBlue;
			nfloat fAlpha;
			UIColor textColor = UIColor.White;
			textColor.GetRGBA (out fRed, out fGreen, out fBlue, out fAlpha);*/
			cGBitmapContext.SetFillColor (red, green, blue, 255);
			cGBitmapContext.SetTextDrawingMode (CGTextDrawingMode.Fill);

			float textX = (float)((1115 - textWidth) / 2f + 285f);
			//cGBitmapContext.TextPosition = new CGPoint (textX, 50);
			cGBitmapContext.ShowTextAtPoint (textX, 20, legend);

			var merge = UIKit.UIImage.FromImage (cGBitmapContext.ToImage ());

			var data = merge.AsPNG ();
			byte [] photoOutputBytes = new Byte [data.Length];
			System.Runtime.InteropServices.Marshal.Copy (data.Bytes, photoOutputBytes, 0, Convert.ToInt32 (data.Length));
			return photoOutputBytes;
		}

	}
}