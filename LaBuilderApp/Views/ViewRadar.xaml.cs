using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaBuilderApp
{

	public class GfxBeacon
	{
		public string Index;
		public int X;
		public int Y;
		public int DestX;
		public int DestY;
		public int Tx;
		public int Ty;
		public string RssiStart;
		public int Life;
		public Image Picture;
	}

	public partial class ViewRadar : ContentView
	{

		bool drawIsDone = false;

		Dictionary<string, GfxBeacon> radar;
		Dictionary<int, string> toRemove = new Dictionary<int, string> ();
		int index = 0;
		Constraint posX;
		Constraint posY;
		Constraint width;
		Constraint height;


		public ViewRadar ()
		{
			InitializeComponent ();
			radar = new Dictionary<string, GfxBeacon> ();
			theLayout.Children.Clear ();
			width = Constraint.RelativeToParent ((parent) => { return 20; });
			height = Constraint.RelativeToParent ((parent) => { return 30; });


			//for (int i = 50; i < 101; i++) {
			//	CalculateY ($"-{i.ToString ()}");
			//}
			Device.StartTimer (TimeSpan.FromMilliseconds (300), DoJob);
		}

		bool DoJob ()
		{
			MoveBeacons ();
			return true;
		}

		private int CalculateX (string key)
		{
			//int num = Int32.Parse (key, System.Globalization.NumberStyles.HexNumber);
			UInt64 num = ulong.Parse (key);
			UInt64 res = num * (ulong)Global.ScreenSize.GetWidth () / 65535;
			string x = num.ToString ("X4");
			Tools.Trace ($"X {x} => {num} => {res}");
			return (int)res;
		}

		private int CalculateY (string rssi)
		{
			// Y -48 => -2 => 0,6 => -0,221848749616356 => 809,091564597102
			// de -47 à _100
			// devient de 47 à 100
			// -40 de 0 à 60 (max)
			// /6  de 0.0 à 10.0
			// +1  de 1.0 à 11.0
			// LOG de 0.0 à LOG(11)
			//
			int num = Convert.ToInt32 (rssi);
			//if (num == -1) return Global.ScreenSize.GetHeight () - 50;
			if (num < 0) num *= -1;
			if (num < 40) return Global.ScreenSize.GetHeight () - 50;
			num -= 40;
			if (num > 60) num = 60;
			double temp = num / 6.0;
			temp++;
			double logTemp = Math.Log10 (temp);
			double res = logTemp * (Global.ScreenSize.GetHeight () - 50.0) / Math.Log10 (11.0);
			res = Global.ScreenSize.GetHeight () - res;
			//Tools.Trace ($"Y {rssi} => {num} => {temp} => {logTemp} => {res}");
			return (int)res;
		}

		private void MoveBeacons ()
		{
			toRemove.Clear ();
			lock (Global.BeaconsLock) {
				foreach (KeyValuePair<string, OneBeacon> kvp in Global.CurrentBeacons) {
					if (radar.ContainsKey (kvp.Key)) {
						radar [kvp.Key].Life = 100;
						if (!radar [kvp.Key].RssiStart.Equals (kvp.Value.Rssi)) {
							radar [kvp.Key].DestY = CalculateY (kvp.Value.Rssi);
						}
					} else {
						GfxBeacon o = CreateBeacon (kvp.Value.Rssi, CalculateX (kvp.Value.Major), CalculateY (kvp.Value.Rssi));
						o.Index = kvp.Key;
						radar.Add (kvp.Key, o);
						posX = Constraint.RelativeToParent ((parent) => { return o.X; });
						posY = Constraint.RelativeToParent ((parent) => { return o.Y; });
						theLayout.Children.Add (o.Picture, posX, posY, width, height);
						Tools.Trace ($"Add beacon {kvp.Key} ({kvp.Value.Rssi}) at {o.X} / {o.Y}");
					}
				}
			}
			int pos = 0;
			foreach (GfxBeacon g in radar.Values) {
				g.Life -= 10;
				g.Picture.Opacity = g.Life / 100.0;
				if (g.Life < 0.1)
					toRemove.Add (pos, g.Index.ToString ());
				else {
					if (g.Y != g.DestY) {
						theLayout.Children [pos].TranslateTo (g.X, g.DestY, 1050, Easing.SinInOut);
						g.Y = g.DestY;
					}
				}
				pos++;
			}
			foreach (KeyValuePair<int, string> kvp in toRemove) {
				radar.Remove (kvp.Value);
				theLayout.Children.RemoveAt (kvp.Key);
				Tools.Trace ($"Lost beacon {kvp.Key} *************");
			}
		}


		public GfxBeacon CreateBeacon (string rssi, int x, int y)
		{
			Image img = new Image ();
			img.WidthRequest = 20;
			img.HeightRequest = 30;
			img.Aspect = Aspect.AspectFit;
			img.Source = ImageSource.FromResource ("LaBuilderApp.Images.r2a6-1-125.png");

			GfxBeacon o = new GfxBeacon ();
			o.Index = (index++).ToString ();
			o.X = x;
			o.Y = y;
			o.DestX = x;
			o.DestY = y;
			o.Tx = 0;
			o.Ty = 0;
			o.Life = 100;
			o.Picture = img;
			o.RssiStart = rssi; // int.Parse (rssi);
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