using System;
using Xamarin.Forms;
using Plugin.ImageCropper;
using Plugin.Media;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using AForge.Imaging.Filters;
using System.Reflection;
using System.Drawing.Imaging;

namespace LaBuilderApp
{
	public partial class ViewCropImage : ContentView
	{
		public ViewCropImage ()
		{
			InitializeComponent ();

			btTest.Clicked += BtTest_Clicked;
		}

		void BtTest_Clicked (object sender, EventArgs e)
		{
			Test ();
		}

		async Task Test ()
		{
			// 1 - on demande de choisir une image

			await CrossMedia.Current.Initialize ();
			/*
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {

				//DisplayAlert ("No Camera", ":( No camera available.", "OK");
				return;
			}

			var file = await CrossMedia.Current.TakePhotoAsync (new Plugin.Media.Abstractions.StoreCameraMediaOptions {
				Directory = "Sample",
				Name = "test.jpg"
			}); */

			var selectedFile = await CrossMedia.Current.PickPhotoAsync ();
			if (selectedFile == null)
				return;

			// 2 - on demande à l'utilisateur de faire un crop

			var cropService = CrossCropImageService.Current;
			var selectedCropFile = await cropService.CropImageFromOriginalToBytes (selectedFile.Path, Plugin.ImageCropper.Abstractions.CropAspect.Custom);
			if (selectedCropFile == null) return;

			// 3 - on charge cette image

			var tempMemoryStream = new MemoryStream (selectedCropFile);
			Bitmap bmpSourceFile = (Bitmap)Bitmap.FromStream (tempMemoryStream);

			// 4 - on la resize dans notre résolution

			ResizeBilinear resize = new ResizeBilinear (1440, 1080);
			Bitmap resizedSourceFile;
			try {
				resizedSourceFile = resize.Apply (bmpSourceFile);
			} catch (Exception err) {
				Tools.Trace ($"Resize error: {err.Message}");
				return;
			}

			CanvasCrop filter = new CanvasCrop (new System.Drawing.Rectangle (
				45, 44, resizedSourceFile.Width - 45 - 46, resizedSourceFile.Height - 44 - 95), System.Drawing.Color.FromArgb (0, 0, 0, 0));
			// apply the filter
			filter.ApplyInPlace (resizedSourceFile);

			// 5 - et dans le format ARGB32

			Bitmap theSourceImage = resizedSourceFile.Clone (new System.Drawing.Rectangle (0, 0, resizedSourceFile.Width, resizedSourceFile.Height), PixelFormat.Format32bppArgb);

			// 6 - on charge notre template

			var assembly = this.GetType ().GetTypeInfo ().Assembly;
			Stream resourceStream = assembly.GetManifestResourceStream ("LaBuilderApp.Images.vierge43.png");
			Bitmap template = (Bitmap)Bitmap.FromStream (resourceStream);

			// 7 - dans le format ARGB32

			Bitmap overlay = template.Clone (new System.Drawing.Rectangle (0, 0, template.Width, template.Height), PixelFormat.Format32bppArgb);

			// 8 - on fusionne les 2 images

			Merge merge = new Merge (overlay);
			Bitmap mergedImages;
			try {
				mergedImages = merge.Apply (theSourceImage);
			} catch (Exception err) {
				Tools.Trace ($"Merge error: {err.Message}");
				return;
			}

			// 9 - on sauvegarde

			tempMemoryStream = new MemoryStream ();
			try {
				mergedImages.Save (tempMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
			} catch (Exception err) {
				Tools.Trace ($"Test7a: {err.Message}");
			}
			tempMemoryStream.Seek (0, SeekOrigin.Begin);

			// 10 - et on affiche à l'écran

			theImage.Source = ImageSource.FromStream (() => { return tempMemoryStream; });

			tempMemoryStream = null;




			//theImage.Source = ImageSource.FromStream (() => new MemoryStream (res));
			/*
			theImage.Source = ImageSource.FromStream (() => {
				var stream = file.GetStream ();
				file.Dispose ();
				return stream;
			);*/

			/*
			Stream ms = new MemoryStream ();
			Tools.Trace ("Test7");
			try {
				bmp.Save (ms, System.Drawing.Imaging.ImageFormat.Png);
			} catch (Exception err) {
				Tools.Trace ($"Test7a: {err.Message}");
			}
			Tools.Trace ("Test7b");
			ms.Seek (0, SeekOrigin.Begin);
			Tools.Trace ("Test8");
			theImage.Source = ImageSource.FromStream (() => { return ms; );
			*/
			/*byte [] buffer;
			using (Stream s = assembly.GetManifestResourceStream ("Images.vierge43.png")) {

			long length = s.Length;
			buffer = new byte [length];
			s.Read (buffer, 0, (int)length)	}*/


		}


	}
}