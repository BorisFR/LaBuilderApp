using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace LaBuilderApp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{

		//[Export ("application:didFinishLaunchingWithOptions:")]
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			LoadApplication (new App ());
			return base.FinishedLaunching (app, options);
		}

	}
}