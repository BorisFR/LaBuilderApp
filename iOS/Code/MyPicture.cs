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
			// create both images
			var data1 = NSData.FromArray (background);
			UIImage img1 = UIImage.LoadFromData (data1);
			var data2 = NSData.FromArray (overlay);
			UIImage img2 = UIImage.LoadFromData (data2);

			// merge both
			// prepare image, with size of overlay
			CGSize newimage = new CGSize (img2.Size.Width, img2.Size.Height);
			// create context for drawing
			var cGBitmapContext = new CGBitmapContext (
				IntPtr.Zero, (int)img2.Size.Width, (int)img2.Size.Height,
				8, (int)(4 * img2.Size.Width),
				CGColorSpace.CreateDeviceRGB (), CGImageAlphaInfo.PremultipliedFirst);
			// destination of background
			var rectBackground = new CGRect (x, img2.Size.Height - height - y, width, height);
			// copy background
			cGBitmapContext.DrawImage (rectBackground, img1.CGImage);
			// size of overlay
			var working = new CGRect (0f, 0f, img2.Size.Width, img2.Size.Height);
			// copy overlay
			cGBitmapContext.DrawImage (working, img2.CGImage);


			// calculate size of text
			var theText = new NSString (legend);
			/*
			cGBitmapContext.SetFillColor (red, green, blue, 255);
			cGBitmapContext.SelectFont ("Roboto-Medium", 60, CGTextEncoding.MacRoman);
			float startX, endX, textWidth;
			startX = (float)cGBitmapContext.TextPosition.X;
			cGBitmapContext.SetTextDrawingMode (CGTextDrawingMode.Invisible);
			cGBitmapContext.ShowText (theText);
			endX = (float)cGBitmapContext.TextPosition.X;
			textWidth = endX - startX;
			float textX = (float)((1115 - textWidth) / 2f + 285f);
			cGBitmapContext.SetTextDrawingMode (CGTextDrawingMode.Fill);*/

			// load font
			var font = UIFont.FromName ("Roboto-Medium", 60);


			var boundSize = new System.Drawing.SizeF ((float)width, float.MaxValue);
			var xx = NSStringDrawingOptions.UsesFontLeading | NSStringDrawingOptions.UsesLineFragmentOrigin;
			var attributes = new UIStringAttributes {
				Font = font
			};
			var sizeF = theText.GetBoundingRect (boundSize, xx, attributes, null).Size;
			float textX = (float)((1115 - sizeF.Width) / 2f + 285f);

			// using current context
			UIGraphics.PushContext (cGBitmapContext);
			// write text in color
			cGBitmapContext.SetFillColor (red, green, blue, 255);
			cGBitmapContext.ScaleCTM (1f, -1f);
			theText.DrawString (new System.Drawing.PointF (textX, -75f), font);
			UIGraphics.PopContext ();

			/*
			nfloat fRed;
			nfloat fGreen;
			nfloat fBlue;
			nfloat fAlpha;
			UIColor textColor = UIColor.White;
			textColor.GetRGBA (out fRed, out fGreen, out fBlue, out fAlpha);*/

			//cGBitmapContext.TextPosition = new CGPoint (textX, 50);
			//cGBitmapContext.ShowTextAtPoint (textX, 20, legend);

			var merge = UIKit.UIImage.FromImage (cGBitmapContext.ToImage ());

			var data = merge.AsPNG ();
			byte [] photoOutputBytes = new Byte [data.Length];
			System.Runtime.InteropServices.Marshal.Copy (data.Bytes, photoOutputBytes, 0, Convert.ToInt32 (data.Length));
			return photoOutputBytes;
		}

	}
}