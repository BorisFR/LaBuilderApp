using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConstructImage
{
	public class GenerateBackground
	{
		public GenerateBackground ()
		{
		}

		public void DoIt (int width, int height)
		{
			System.Drawing.Image toto = new Bitmap (width, height, PixelFormat.Format32bppArgb);
			Graphics drawing = Graphics.FromImage (toto);

			drawing.Clear (Color.Black);

			Pen pBlue = new Pen (Color.FromArgb (100, 90, 170, 210));

			int i = 0;
			System.Diagnostics.Debug.WriteLine ("Doing width");
			while (i < width) {
				drawing.DrawLine (pBlue, i, 0, i, height);
				i = i + 10;
			}

			i = 0;
			System.Diagnostics.Debug.WriteLine ("Doing height");
			while (i < height) {
				drawing.DrawLine (pBlue, 0, i, width, i);
				i = i + 10;
			}

			System.Diagnostics.Debug.WriteLine ("Saving");
			drawing.Save ();
			System.Diagnostics.Debug.WriteLine ("To x.png");
			toto.Save ("x.png", ImageFormat.Png);
		}

	}
}