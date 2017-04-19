using System;
using Xamarin.Forms;
using System.Net;
using System.Globalization;
using Plugin.Settings;
using System.Collections.Generic;

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
		About,
		Connect,
		Medias,
		Things,
		Legal,
		GameR2Finder,
		GameRuzzle,
		Culture,
		Alphabet,
		Radar,
		CropCard,
		ListCards
	}

	public delegate void Trigger ();
	public delegate void TriggerObject (object sender, object e);
	public delegate void JobDone (object sender, bool status, string result);
	public delegate void ScoreDone (bool isFirst, int game, int level, int score, bool status, string result);


	public static class Global
	{
		public static MenuPage MenuPage;
		public static DetailPage DetailPage;
		public static MainAppPage MainAppPage;
		public static MenuManager Menus = new MenuManager ();
		public static readonly Thickness PagePadding = new Thickness (Device.OnPlatform (0, 0, 0), Device.OnPlatform (20, 0, 0), Device.OnPlatform (0, 0, 0), Device.OnPlatform (0, 0, 0));

		public static bool OnSleep = false;
		public static bool IsResume = false;
		public static bool IsDoingInit = false;
		public static Menu CurrentMenu = null;

		public static bool IsConnected = false;
		//public static User ConnectedUser = null;


		public static string BaseUrl = "https://www.r2builders.fr/";
		public static string AppUrl = "https://www.r2builders.fr/DroidBuilders.Fr/";
		public static string DataUrl = "https://www.r2builders.fr/boris/data/";
		public static CultureInfo CultureFrench = new CultureInfo ("fr-FR");

		// Builders
		//public static string iBeaconUUID = "E5CAF8CF-590C-42DC-9CF0-2929552156A7"; 
		// Estimote Maxxing
		public static string iBeaconUUID = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
		//public static string iBeaconRegion = "Builders";
		public static string iBeaconRegion = "Estimote";
		public static BeaconStuff JobBeacon = new BeaconStuff ();

		public static IFiles Files = null;
		public static IScreenSize ScreenSize = null;
		public static IBeacons Beacons = null;
		public static IMyPicture MyPicture = null;

		public static Random Random;
		public static Color DarkColor = Color.FromHex ("#11252D");
		public static Color LightColor = Color.FromHex ("#5AA9D3");

		public static Exhibition SelectedExhibition;
		public static string ComingFromEvent = string.Empty;
		public static Builder SelectedBuilder;
		public static int ComingFromBuilder = 0;
		public static Thing SelectedThing;
		public static string ComingFromThing = string.Empty;

		public static string CurrentLogin = string.Empty;
		public static string CurrentPassword = string.Empty;
		public static string CurrentToken = string.Empty;
		public static int CurrentBuilderId = 0;

		// pour exécuter un re-auth automatique si jamais on s'est connecté avec un autre device. Le token n'est plus bon.
		public static bool IsFirstTimeError = true;

		public static async void DoInit (string from)
		{
			if (Files != null) {
				Tools.Trace ($"> {from}: Global.DoInit() already done.");
				IsDoingInit = false;
				return;
			}
			IsDoingInit = true;
			Files = DependencyService.Get<IFiles> ();
			ScreenSize = DependencyService.Get<IScreenSize> ();
			Beacons = DependencyService.Get<IBeacons> ();
			MyPicture = DependencyService.Get<IMyPicture> ();
			Random = new Random (DateTime.Now.Millisecond);
			CurrentLogin = CrossSettings.Current.GetValueOrDefault<string> ("userlogin", string.Empty);
			CurrentPassword = CrossSettings.Current.GetValueOrDefault<string> ("userpassword", string.Empty);
			CurrentToken = CrossSettings.Current.GetValueOrDefault<string> ("usertoken", string.Empty);
			if (CurrentPassword == string.Empty)
				CurrentToken = string.Empty;
			JobBeacon.DoInit ();
			MenuManager.Refresh ();
			Tools.ClearAllWinners ();
			Beacons.FoundBeacons += Beacons_FoundBeacons;
			Beacons.BeaconInfo += Beacons_BeaconInfo;
			Beacons.Init (iBeaconUUID, iBeaconRegion);
			Beacons.Start ();
			Tools.Trace ($"> {from}: Global.DoInit() done.");
		}

		public static void GotoPage (Menu menu)
		{
			//AwesomeWrappanel.CleanAll ();
			DetailPage.ShowPage (menu);
		}

		public static void ClearDataCache ()
		{
			CrossSettings.Current.Remove ("usertoken");
			CrossSettings.Current.Remove ("userpassword");
			IDataServer.ClearData ("builders");
			IDataServer.ClearData ("things");
			IDataServer.ClearData ("thingstype");
			IDataServer.ClearData ("events");
			IDataServer.ClearData ("country");
			IDataServer.ClearData ("cards");
			Tools.ClearAllWinners ();
			JobBeacon.ClearCache ();
		}

		static void Beacons_FoundBeacons (System.Collections.Generic.List<LaBuilderApp.OneBeacon> beacons)
		{
			JobBeacon.FoundBeacons (beacons);

		}

		static void Beacons_BeaconInfo (string text)
		{
			Tools.Trace ($"******************************** Beacons Info: {text}");
		}

	}
}