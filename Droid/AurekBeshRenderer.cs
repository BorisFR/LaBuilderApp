using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using LaBuilderApp.Droid;
using LaBuilderApp;

[assembly: ExportRenderer (typeof (AurekBesh), typeof (AurekBeshRenderer))]

namespace LaBuilderApp.Droid
{
	public class AurekBeshRenderer : LabelRenderer
	{
		public static Typeface CacheAurekBesh = Typeface.CreateFromAsset (Forms.Context.Assets, AurekBesh.Typeface + ".ttf");

		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);
			//if (e.OldElement == null)
			{
				//if(CacheAurekBesh == null)
				//	CacheAurekBesh = Typeface.CreateFromAsset(Forms.Context.Assets, AurekBesh.Typeface + ".ttf");
				//The ttf in /Assets is CaseSensitive
				Control.Typeface = CacheAurekBesh;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			Control.Typeface = CacheAurekBesh;
		}

	}
}