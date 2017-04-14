using System;
using Xamarin.Forms;
using Plugin.ImageCropper;
using Plugin.Media;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

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

			theImage.Source = ImageSource.FromResource ("LaBuilderApp.Images.1x1clear.png");

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
			byte [] background = tempMemoryStream.ToArray ();

			// 3 - on charge notre template

			var assembly = this.GetType ().GetTypeInfo ().Assembly;
			Stream resourceStream = assembly.GetManifestResourceStream ("LaBuilderApp.Images.vierge43.png");
			MemoryStream msxx = new MemoryStream ();
			resourceStream.CopyTo (msxx);
			byte [] overlay = msxx.ToArray ();


			// 8 - on fusionne les 2 images
			// en précisant la zone à remplir de l'overlay

			//byte [] merged = Global.MyPicture.DoMerge (background, overlay, 45, 44, 1440 - 45 - 46, 1080 - 44 - 95);
			//byte [] merged = Global.MyPicture.DoMerge (background, overlay, 0, 0, 1440, 1080);
			string text = string.Empty;
			if (theText != null && theText.Text != null)
				text = theText.Text.Trim ();
			var merged = Global.MyPicture.DoMerge (background, overlay, 45, 44, 1440 - 45 - 46, 1080 - 44 - 95, text, 255, 255, 255);
			theImage.Source = ImageSource.FromStream (() => new MemoryStream (merged));
		}


	}
}