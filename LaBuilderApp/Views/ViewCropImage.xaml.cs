using System;
using Xamarin.Forms;
using Plugin.ImageCropper;
using Plugin.Media;
using System.Threading.Tasks;
using System.IO;

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
			await CrossMedia.Current.Initialize ();
			/*
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {

				//DisplayAlert ("No Camera", ":( No camera available.", "OK");
				return;
			}

			var file = await CrossMedia.Current.TakePhotoAsync (new Plugin.Media.Abstractions.StoreCameraMediaOptions {
				Directory = "Sample",
				Name = "test.jpg"
			});
*/
			var file = await CrossMedia.Current.PickPhotoAsync ();
			if (file == null)
				return;

			var x = CrossCropImageService.Current;

			var res = await x.CropImageFromOriginalToBytes (file.Path, Plugin.ImageCropper.Abstractions.CropAspect.Custom);
			if (res == null) return;

			theImage.Source = ImageSource.FromStream (() => new MemoryStream (res));
			/*
			theImage.Source = ImageSource.FromStream (() => {
				var stream = file.GetStream ();
				file.Dispose ();
				return stream;
			});*/

			var imageSource = ImageSource.FromResource ("LaBuilderApp.Images.vierge43.png");



		}


	}
}