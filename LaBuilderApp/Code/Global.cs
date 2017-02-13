using System;
using Xamarin.Forms;
using System.Net;
using System.Globalization;

namespace LaBuilderApp
{
	public enum MyPage
	{
		None,
		FirstLoading,
		News,
		Builders,
		Events,
		Account,
		MyBuilder,
		AdminUsers,
		AdminBuilders,
		MyExhibitions,
		About
	}

	public delegate void Trigger ();
	public delegate void JobDone (object sender, bool status, string result);


	public static class Global
	{
		public static MenuPage MenuPage;
		public static DetailPage DetailPage;
		public static MainAppPage MainAppPage;
		public static MenuManager Menus = new MenuManager ();
		public static readonly Thickness PagePadding = new Thickness (Device.OnPlatform (0, 0, 0), Device.OnPlatform (20, 0, 0), Device.OnPlatform (0, 0, 0), Device.OnPlatform (0, 0, 0));

		public static bool IsConnected = false;
		//public static User ConnectedUser = null;


		public static string BaseUrl = "http://r2builders.fr/";
		public static CultureInfo CultureFrench = new CultureInfo ("fr-FR");

		public static IFiles Files = null;

		public static Random Random;
		public static Color DarkColor = Color.FromHex ("#11252D");
		public static Color LightColor = Color.FromHex ("#5AA9D3");

		public static Exhibition SelectedExhibition;
		public static Builder SelectedBuilder;

		public static async void DoInit (string from)
		{
			if (Files != null) {
				Tools.Trace ($"> {from}: Global.DoInit() already done.");
				return;
			}
			Files = DependencyService.Get<IFiles> ();
			Random = new Random (DateTime.Now.Millisecond);
			/*
			IDataServer events = new IDataServer ("events");
			//events.IgnoreLocalData = true;
			events.DataRefresh += (status, result) => {
				Tools.Trace ("DataRefresh: " + result);
				//Exhibition.All = Exhibition.LoadData (result);
				Exhibition.LoadData (result);
				Exhibition.PopulateData ();
			};
			DataServer.AddToDo (events);

			DataServer.Launch ();*/
			MenuManager.Refresh ();
			Tools.Trace ($"> {from}: Global.DoInit() done.");
		}

		public static void GotoPage (Menu menu)
		{
			//AwesomeWrappanel.CleanAll ();
			DetailPage.ShowPage (menu);
		}

	}
}