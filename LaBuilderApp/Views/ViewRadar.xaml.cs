using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{

	public enum BeaconImageType
	{
		Far,    // trés loin
		Medium, // entre proche et loin
		Near,   // proche
		Found,  // trouvé
		None    // pas d'état
	}

	public class GfxBeacon
	{
		public string Key;
		public string Index;
		public int X;           // position initiale
		public int Y;
		public int DestX;       // nouvelle position
		public int DestY;
		public int RssiStart;   // rssi initial
		public int LastRssi;    // rssi actuel
		public int Life;        // durée de vie
		public Image Picture;   // image à afficher
		public BeaconImageType BeaconImageType; // type de l'image affichée
	}

	public partial class ViewRadar : ContentView
	{

		//const int MAXHEIGHT = 400;
		const int MINRSSI = 50;
		const int MAXRSSI = 100;
		const int LIMITRSSI = 87;
		int ecartRssiLow = LIMITRSSI - MINRSSI;
		int ecartRssiHigh = MAXRSSI - LIMITRSSI;
		int fixChildrens = 0;

		private int fullHeight;
		private int height13;
		private int height23;
		private int height23demi;

		bool drawIsDone = false;

		Dictionary<string, GfxBeacon> radar;
		Dictionary<int, string> toRemove = new Dictionary<int, string> ();

		int index = 0;
		//Constraint posX;
		//Constraint posY;
		//Constraint width;
		//Constraint height;

		// 656B.6715 -87 25963 148
		// 2087.2D63 -87 8327	47

		// 2087.2D63 -86
		// 656B.6715 0

		// 2087.2D63 -86
		// 656B.6715 0

		// 2087.2D63 -87
		// 7D4C.EE84 -90 32076 183
		// 85E1.940C -95 34273 196

		public void GenerateFakeBeacons ()
		{
			Global.JobBeacon.CurrentBeacons.Clear ();
			/*
			Global.JobBeacon.CurrentBeacons.Add ("1", new OneBeacon () { Major = $"{0000}", Minor = "2F01", Rssi = "-40" });
			Global.JobBeacon.CurrentBeacons.Add ("1A", new OneBeacon () { Major = $"{3000}", Minor = "2F01", Rssi = "-41" });
			Global.JobBeacon.CurrentBeacons.Add ("1B", new OneBeacon () { Major = $"{4000}", Minor = "2F01", Rssi = "-42" });
			Global.JobBeacon.CurrentBeacons.Add ("2", new OneBeacon () { Major = $"{5000}", Minor = "2F01", Rssi = "-45" });

			Global.JobBeacon.CurrentBeacons.Add ("3", new OneBeacon () { Major = $"{10000}", Minor = "2F01", Rssi = "-50" });
			Global.JobBeacon.CurrentBeacons.Add ("4", new OneBeacon () { Major = $"{15000}", Minor = "2F01", Rssi = "-55" });
			Global.JobBeacon.CurrentBeacons.Add ("5", new OneBeacon () { Major = $"{20000}", Minor = "2F01", Rssi = "-60" });

			Global.JobBeacon.CurrentBeacons.Add ("10", new OneBeacon () { Major = $"{25000}", Minor = "2F01", Rssi = "-65" });
			Global.JobBeacon.CurrentBeacons.Add ("11", new OneBeacon () { Major = $"{28000}", Minor = "2F01", Rssi = "-70" });
			Global.JobBeacon.CurrentBeacons.Add ("12", new OneBeacon () { Major = $"{32000}", Minor = "2F01", Rssi = "-71" });
			Global.JobBeacon.CurrentBeacons.Add ("13", new OneBeacon () { Major = $"{35000}", Minor = "2F01", Rssi = "-72" });
			Global.JobBeacon.CurrentBeacons.Add ("14", new OneBeacon () { Major = $"{38000}", Minor = "2F01", Rssi = "-73" });
			Global.JobBeacon.CurrentBeacons.Add ("15", new OneBeacon () { Major = $"{42000}", Minor = "2F01", Rssi = "-74" });
			Global.JobBeacon.CurrentBeacons.Add ("16", new OneBeacon () { Major = $"{45000}", Minor = "2F01", Rssi = "-75" });

			return;
			*/
			for (int i = 40; i <= 100; i += 1) {
				Global.JobBeacon.CurrentBeacons.Add ($"{i}", new OneBeacon () { Major = $"{(i - 39) * 800}", Minor = "2F01", Rssi = $"-{i}" });
			}
			//Global.CurrentBeacons.Add ("656B", new OneBeacon () { Major = "25963", Minor = "2F01", Rssi = $"-{50 + Global.Random.Next (30)}" });
			return;
			switch (Global.Random.Next (10)) {
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
				Global.JobBeacon.CurrentBeacons.Add ("7D4C", new OneBeacon () { Major = "32076", Minor = "2F01", Rssi = $"-{40 + Global.Random.Next (60)}" });
				break;
			case 5:
			case 6:
				Global.JobBeacon.CurrentBeacons.Add ("656B", new OneBeacon () { Major = "25963", Minor = "2F01", Rssi = $"-{40 + Global.Random.Next (60)}" });
				Global.JobBeacon.CurrentBeacons.Add ("2087", new OneBeacon () { Major = "8327", Minor = "2F01", Rssi = $"-{40 + Global.Random.Next (60)}" });
				break;
			case 7:
			case 8:
			case 9:
				Global.JobBeacon.CurrentBeacons.Add ("656B", new OneBeacon () { Major = "25963", Minor = "2F01", Rssi = $"-{40 + Global.Random.Next (60)}" });
				Global.JobBeacon.CurrentBeacons.Add ("85E1", new OneBeacon () { Major = "34273", Minor = "2F01", Rssi = $"-{40 + Global.Random.Next (60)}" });
				break;
			}
		}

		public ViewRadar ()
		{
			InitializeComponent ();
			radar = new Dictionary<string, GfxBeacon> ();
			theLayout.Children.Clear ();

			fullHeight = Global.ScreenSize.GetHeight () - 100;
			//fullHeight = MAXHEIGHT;
			height13 = fullHeight / 3;
			height23 = fullHeight * 2 / 3;
			height23demi = height23 / 2;
			Tools.Trace ($"full: {fullHeight} = 1/3 {height13} + 2/3 {height23}");
			log133 = Math.Log10 (1.0 / (LIMITRSSI - MINRSSI));
			log127 = Math.Log10 (1.0 / (MAXRSSI - LIMITRSSI));
			Tools.Trace ($"log: {log133} / {log127}");

			BoxView box = new BoxView ();
			box.BackgroundColor = Color.Aqua;
			box.HeightRequest = 1;
			box.WidthRequest = Global.ScreenSize.GetWidth ();
			theLayout.Children.Add (box, new Point (0, height13));
			fixChildrens++;

			box = new BoxView ();
			box.BackgroundColor = Color.White;
			box.HeightRequest = 1;
			box.WidthRequest = Global.ScreenSize.GetWidth ();
			theLayout.Children.Add (box, new Point (0, 0));
			fixChildrens++;

			box = new BoxView ();
			box.BackgroundColor = Color.White;
			box.HeightRequest = 1;
			box.WidthRequest = Global.ScreenSize.GetWidth ();
			theLayout.Children.Add (box, new Point (0, fullHeight));
			fixChildrens++;

			box = new BoxView ();
			box.BackgroundColor = Color.Green;
			box.HeightRequest = 1;
			box.WidthRequest = Global.ScreenSize.GetWidth ();
			theLayout.Children.Add (box, new Point (0, CalculateY ("-56")));
			fixChildrens++;


			//width = Constraint.RelativeToParent ((parent) => { return 32; });
			//height = Constraint.RelativeToParent ((parent) => { return 32; });
			/*
			theLayout.SizeChanged += (sender, e) => {
				fullHeight = (int)theLayout.Height - 50;
				height13 = fullHeight / 3;
				height23 = fullHeight * 2 / 3;
				Tools.Trace ($"full: {fullHeight} = 1/3 {height13} + 2/3 {height23} ");
			};
			*/
			//for (int i = 50; i < 101; i++) {
			//	CalculateY ($"-{i.ToString ()}");
			//}
			Device.StartTimer (TimeSpan.FromMilliseconds (300), DoJob);
		}

		bool DoJob ()
		{
			//if (Global.Random.Next (10) > 3)
			//GenerateFakeBeacons ();
			MoveBeacons ();
			return true;
		}

		private int CalculateX (string key)
		{
			//int num = Int32.Parse (key, System.Globalization.NumberStyles.HexNumber);
			UInt64 num = ulong.Parse (key);
			UInt64 res = num * (ulong)Global.ScreenSize.GetWidth () / 65535;
			string x = num.ToString ("X4");
			//Tools.Trace ($"X {x} => {num} => {res}");
			return (int)res;
		}

		private double res;
		private double logTemp;
		private double temp;
		private double log133;
		private double log127;

		private int CalculateY (string rssi)
		{
			int num = Convert.ToInt32 (rssi);
			// test 1
			// ------
			// Y -48 => -2 => 0,6 => -0,221848749616356 => 809,091564597102
			// de -47 à _100
			// devient de 47 à 100
			// -40 de 0 à 60 (max)
			// /6  de 0.0 à 10.0
			// +1  de 1.0 à 11.0
			// LOG de 0.0 à LOG(11)
			//
			/*
			if (num < 0) num *= -1;
			if (num < 40) return 10;
			num -= 40;
			if (num > 60) num = 60;
			double temp = num / 6.0;
			temp++;
			double logTemp = Math.Log10 (temp);
			double res = logTemp * (Global.ScreenSize.GetHeight () - 50.0) / Math.Log10 (11.0);
			res = Global.ScreenSize.GetHeight () - res;
			//Tools.Trace ($"Y {rssi} => {num} => {temp} => {logTemp} => {res}");
			return (int)res;
			*/

			// test 2
			// ------
			// de (0) 40 à 69  : proche
			//	transforme en entre 0.1 et 1
			//	donne x entre log(0.1) = -1 et 0
			//	transforme en entre 0 et 2/3 height

			// de 70 à 100 (+) : loin
			//	transforme entre 1 et 10
			//	donne x entre 0 et 1=Log(10)
			//	transforme entre 2/3 height et 3/3 height

			if (num < 0) num *= -1;
			if (num < MINRSSI) num = MINRSSI;
			if (num > MAXRSSI) num = MAXRSSI;
			num -= MINRSSI;
			if (num <= ecartRssiLow) {
				temp = (num + 0.01) / ecartRssiLow; // => de 0 à 1
				logTemp = Math.Log10 (temp);
				temp = height13 * logTemp;
				res = temp / log133;
				if (res > height23demi) res = height23demi;
				res *= 1.5;
				Tools.Trace ($"Y {rssi} => {num} => {logTemp} => {temp} => {res}");
				return (int)res + height13;
			}
			num -= ecartRssiLow;
			temp = (num + 0.01) / ecartRssiHigh;
			logTemp = Math.Log10 (1 + temp);
			temp = logTemp * height13;
			temp *= 1.5;
			//res = temp / log127;
			Tools.Trace ($"Y {rssi} => {num} => {logTemp} => {temp}"); // => {res}");
			if (max > Convert.ToInt32 (temp)) {
				max = (int)temp;
				Tools.Trace ($"{rssi} => max= {max}");
			}
			return height13 - (int)(temp);
		}
		private int max = 1000;

		private void MoveBeacons ()
		{
			toRemove.Clear ();
			lock (Global.JobBeacon.BeaconsLock) {
				foreach (KeyValuePair<string, OneBeacon> kvp in Global.JobBeacon.CurrentBeacons) {
					if (radar.ContainsKey (kvp.Key)) {
						radar [kvp.Key].Life = 100;
						radar [kvp.Key].LastRssi = int.Parse (kvp.Value.Rssi);
						if (!radar [kvp.Key].RssiStart.Equals (kvp.Value.Rssi)) {
							radar [kvp.Key].DestY = CalculateY (kvp.Value.Rssi);
							//if (kvp.Key.Equals ("85E1.940C")) {
							//Tools.Trace ($"{kvp.Value.Rssi} - {radar [kvp.Key].Y} > {radar [kvp.Key].DestY} : {radar [kvp.Key].DestY - radar [kvp.Key].Y} {kvp.Value.Description}");
							//}
						}
					} else {
						GfxBeacon o = CreateBeacon (kvp.Value.Rssi, CalculateX (kvp.Value.Major), CalculateY (kvp.Value.Rssi));
						o.Index = kvp.Key;
						o.LastRssi = int.Parse (kvp.Value.Rssi);
						o.Key = kvp.Key;
						if (Global.JobBeacon.FoundedBeacons.Contains (kvp.Key))
							o.BeaconImageType = ChangeImage (o.Picture, o.LastRssi, BeaconImageType.Found);
						radar.Add (kvp.Key, o);
						//posX = Constraint.RelativeToParent ((parent) => { return o.X; });
						//posY = Constraint.RelativeToParent ((parent) => { return o.Y; });
						//theLayout.Children.Add (o.Picture, posX, posY, width, height);
						theLayout.Children.Add (o.Picture, new Point (o.X, o.Y));
						Tools.Trace ($"Add beacon {kvp.Key} ({kvp.Value.Rssi}) at {o.X} / {o.Y} {kvp.Value.Description}");
					}
				}
			}
			int pos = fixChildrens;
			foreach (GfxBeacon g in radar.Values) {
				g.Life -= 1;
				g.Picture.Opacity = g.Life / 100.0;
				if (g.Life < 0.1)
					toRemove.Add (pos, g.Index);
				else {
					//if (g.BeaconImageType != BeaconImageType.Found) {
					g.BeaconImageType = ChangeImage ((Image)theLayout.Children [pos], g.LastRssi, g.BeaconImageType);
					//}
					if (g.Y != g.DestY) {
						//if (g.Index.Equals ("85E1.940C")) {
						//	Tools.Trace ($"To {g.DestY} : {g.DestY - g.Y}");
						//}
						//Tools.Trace ($"Move to {g.X} / {g.DestY}");
						theLayout.Children [pos].TranslateTo (0, g.DestY - g.Y, 1500, Easing.SinInOut);
						//g.Y = g.DestY;
					}
				}
				pos++;
			}
			pos = 0;
			foreach (KeyValuePair<int, string> kvp in toRemove) {
				try {
					radar.Remove (kvp.Value);
					theLayout.Children.RemoveAt (kvp.Key + pos--);
					//Tools.Trace ($"Lost beacon {kvp.Key} *************");
				} catch (Exception err) {
					Tools.Trace ($"***************************** Error removing beacon {kvp.Key} *************");
				}
			}
		}

		private BeaconImageType ChangeImage (Image img, int rssi, BeaconImageType actual)
		{
			if (actual == BeaconImageType.Found) {
				img.Source = ImageSource.FromResource ("LaBuilderApp.Images.circle_green.png");
				return BeaconImageType.Found;
			}
			if (rssi >= -BeaconStuff.FOUNDRSSI) {
				if (actual == BeaconImageType.Found) return BeaconImageType.Found;
				img.Source = ImageSource.FromResource ("LaBuilderApp.Images.circle_green.png");
				return BeaconImageType.Found;
			}

			if (rssi < -LIMITRSSI) {
				if (actual == BeaconImageType.Far) return BeaconImageType.Far;
				img.Source = ImageSource.FromResource ("LaBuilderApp.Images.circle_red.png");
				return BeaconImageType.Far;
			}

			if (rssi >= -LIMITRSSI) {
				if (actual == BeaconImageType.Near) return BeaconImageType.Near;
				img.Source = ImageSource.FromResource ("LaBuilderApp.Images.circle_yellow.png");
				return BeaconImageType.Near;
			}
			if (actual == BeaconImageType.Medium) return BeaconImageType.Medium;
			img.Source = ImageSource.FromResource ("LaBuilderApp.Images.circle_yellow.png");
			return BeaconImageType.Medium;
		}


		public GfxBeacon CreateBeacon (string rssi, int x, int y)
		{
			Image img = new Image ();
			img.WidthRequest = 32;
			img.HeightRequest = 32;
			img.Aspect = Aspect.AspectFit;
			//if (Global.Random.Next (10) > 4)
			/*
			if (rssi.CompareTo ($"-{LIMITRSSI}") > 0)
				img.Source = ImageSource.FromResource ("LaBuilderApp.Images.circle_red.png");
			else
				img.Source = ImageSource.FromResource ("LaBuilderApp.Images.circle_green.png");
			*/
			GfxBeacon o = new GfxBeacon ();
			o.Index = (index++).ToString ();
			o.X = x;
			o.Y = y;
			o.DestX = x;
			o.DestY = y;
			o.Life = 100;
			o.Picture = img;
			o.RssiStart = int.Parse (rssi);
			o.BeaconImageType = ChangeImage (o.Picture, o.RssiStart, BeaconImageType.None);
			return o;
		}

		protected override void OnSizeAllocated (double width, double height)
		{
			if (width > 0 && height > 0)
				theLayout.HeightRequest = Global.ScreenSize.GetHeight ();
			base.OnSizeAllocated (width, height);
		}

	}
}