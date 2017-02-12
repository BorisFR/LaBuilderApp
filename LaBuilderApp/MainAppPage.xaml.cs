using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace LaBuilderApp
{
	public partial class MainAppPage : MasterDetailPage
	{
		public MainAppPage ()
		{
			InitializeComponent ();
			Global.MainAppPage = this;
			Global.MenuPage = new MenuPage ();
			this.Master = Global.MenuPage;
			Global.DetailPage = new DetailPage ();
			this.Detail = new NavigationPage (Global.DetailPage);

			Tools.Trace ("MainAppPage done.");
			if (Device.OS == TargetPlatform.Windows)
				this.MasterBehavior = MasterBehavior.Split;
		}
	}
}