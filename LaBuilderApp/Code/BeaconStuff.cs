using System;
using System.Collections.Generic;
using Plugin.Settings;

namespace LaBuilderApp
{
	public class BeaconStuff
	{

		public Dictionary<string, OneBeacon> CurrentBeacons = new Dictionary<string, OneBeacon> ();
		public Dictionary<string, OneBeacon> ViewedBeacons = new Dictionary<string, OneBeacon> ();
		public object BeaconsLock = new Object ();

		private string major;
		private string minor;
		private string id;
		private bool foundBeaconRegion = false;

		public BeaconStuff ()
		{
		}


		private string ToHex4 (string text)
		{
			return Convert.ToInt32 (text).ToString ("X4");
		}

		public void DoInit ()
		{
			DateTime d = CrossSettings.Current.GetValueOrDefault<DateTime> ("FoundBeaconRegion", new DateTime (2000, 1, 1));
			if (d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day) {
				foundBeaconRegion = true;
			}

		}

		public void FoundBeacons (List<OneBeacon> beacons)
		{
			if (!foundBeaconRegion) {
				foundBeaconRegion = true;
				CrossSettings.Current.AddOrUpdateValue<DateTime> ("FoundBeaconRegion", DateTime.Now);
				Global.MainAppPage.DisplayAlert ("Droid Builders .Fr", "Il y a des Droid Builders dans les environs !", "Ok");
			}
			lock (BeaconsLock) {
				CurrentBeacons.Clear ();
				foreach (OneBeacon b in beacons) {
					if (b.Rssi.Equals ("0")) continue;
					major = ToHex4 (b.Major);
					minor = ToHex4 (b.Minor);
					id = major + "." + minor;
					if (ViewedBeacons.ContainsKey (id))
						ViewedBeacons [id] = b;
					else ViewedBeacons.Add (id, b);
					if (CurrentBeacons.ContainsKey (id))
						CurrentBeacons [id] = b;
					else CurrentBeacons.Add (id, b);
					//Tools.Trace ($"******************************** Beacons: {id} {b.Rssi}");
				}
			}
		}

	}
}