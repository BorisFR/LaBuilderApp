using System;
using System.IO;
using Android.Graphics;
using Android.Text;
using Java.IO;
using Java.Nio;
using LaBuilderApp.Droid;
using Xamarin.Forms;

[assembly: Dependency (typeof (MyPicture))]

namespace LaBuilderApp.Droid
{
	public class MyPicture : IMyPicture
	{
		public byte [] DoMerge (byte [] background, byte [] overlay, int x, int y, int width, int height, string legend, int red, int green, int blue)
		{
			// https://forums.xamarin.com/discussion/37647/cross-platform-crop-image-view

			// read info du background
			BitmapFactory.Options bfOptions = new BitmapFactory.Options ();
			bfOptions.InJustDecodeBounds = true;
			BitmapFactory.DecodeByteArray (background, 0, background.Length, bfOptions);
			int bgWidth = bfOptions.OutWidth;
			int bgHeight = bfOptions.OutHeight;
			//String imageType = bfOptions.OutMimeType;

			// read info de l'overlay
			bfOptions = new BitmapFactory.Options ();
			bfOptions.InJustDecodeBounds = true;
			BitmapFactory.DecodeByteArray (overlay, 0, overlay.Length, bfOptions);
			int ovWidth = bfOptions.OutWidth;
			int ovHeight = bfOptions.OutHeight;

			// on charge le background
			bfOptions = new BitmapFactory.Options ();
			var imgBackground = BitmapFactory.DecodeByteArray (background, 0, background.Length, bfOptions);

			// on charge l'overlay
			bfOptions = new BitmapFactory.Options ();
			var imgOverlay = BitmapFactory.DecodeByteArray (overlay, 0, overlay.Length, bfOptions);


			Bitmap merged = null;
			try {
				merged = Bitmap.CreateBitmap (ovWidth, ovHeight, Bitmap.Config.Argb8888);
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ($"Error: {err.Message}");
			}

			Canvas canvas = new Canvas (merged);
			Paint paint = new Paint ();
			paint.Color = Android.Graphics.Color.White;
			paint.SetStyle (Paint.Style.Fill);
			canvas.DrawPaint (paint);

			Rect source = new Rect (0, 0, bgWidth, bgHeight);
			Rect destination = new Rect (x, y, width + x, height + y);
			paint = new Paint ();
			paint.AntiAlias = true;
			canvas.DrawBitmap (imgBackground, source, destination, paint);
			canvas.DrawBitmap (imgOverlay, 0f, 0f, paint);

			TextPaint text = new TextPaint ();
			text.Color = new Android.Graphics.Color (red, green, blue);
			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "Roboto-Medium.ttf");
			text.SetTypeface (font);
			text.TextSize = 60;
			text.AntiAlias = true;
			float textWidth = text.MeasureText (legend);
			float textX = (float)((1115f - textWidth) / 2f + 285f);
			canvas.DrawText (legend, textX, 1080f - 19f, text);


			var photoOutputStream = new MemoryStream ();
			merged.Compress (Bitmap.CompressFormat.Png, 100, photoOutputStream);
			return photoOutputStream.ToArray ();
			/*
			int bytes = merged.RowBytes * merged.Height; //  merged.AllocationByteCount;
			ByteBuffer buffer = ByteBuffer.Allocate (bytes);
			merged.CopyPixelsToBuffer (buffer);
			buffer.Rewind ();
			byte [] res = new byte [bytes];
			try {
				buffer.Get (res, 0, bytes);
				//res = buffer.ToArray<byte> ();
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ($"Error: {err.Message}");
			}
			return res; */
			/*
			var photoOutputStream = new MemoryStream ();
			merged.Compress (Bitmap.CompressFormat.Png, 100, photoOutputStream);

			return photoOutputStream.ToArray ();*/
		}

		/*
		private int CalculateInSampleSize (int inputWidth, int inputHeight, int outputWidth, int outputHeight)
		{
			//see http://developer.android.com/training/displaying-bitmaps/load-bitmap.html

			int inSampleSize = 1;       //default

			if (inputHeight > outputHeight || inputWidth > outputWidth) {

				int halfHeight = inputHeight / 2;
				int halfWidth = inputWidth / 2;

				// Calculate the largest inSampleSize value that is a power of 2 and keeps both
				// height and width larger than the requested height and width.
				while ((halfHeight / inSampleSize) > outputHeight && (halfWidth / inSampleSize) > outputWidth) {
					inSampleSize *= 2;
				}
			}

			return inSampleSize;
		} */

	}
}