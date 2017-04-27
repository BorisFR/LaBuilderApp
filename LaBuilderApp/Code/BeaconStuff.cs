using System;
using System.Collections.Generic;
using Plugin.Settings;
using System.Collections.ObjectModel;

namespace LaBuilderApp
{
	public class BeaconStuff
	{
		public static int FOUNDRSSI = 57;

		public Dictionary<string, OneBeacon> CurrentBeacons = new Dictionary<string, OneBeacon> ();
		//public Dictionary<string, OneBeacon> ViewedBeacons = new Dictionary<string, OneBeacon> ();
		public List<string> FoundedBeacons = new List<string> ();
		public object BeaconsLock = new Object ();

		public static ObservableCollection<OneBeacon> AllVisible = new ObservableCollection<OneBeacon> ();
		public static ObservableCollection<string> AllFound = new ObservableCollection<string> ();

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
			AllVisible.Clear ();
			AllFound.Clear ();
			DateTime d = CrossSettings.Current.GetValueOrDefault<DateTime> ("FoundBeaconRegion", new DateTime (2000, 1, 1));
			if (d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day) {
				foundBeaconRegion = true;
			}
			d = CrossSettings.Current.GetValueOrDefault<DateTime> ("FoundedBeacons", new DateTime (2000, 1, 1));
			if (d.Year == DateTime.Now.Year && d.Month == DateTime.Now.Month && d.Day == DateTime.Now.Day) {
				string s = CrossSettings.Current.GetValueOrDefault<string> ("FoundedBeaconsList", string.Empty);
				string [] st = s.Split (',');
				FoundedBeacons.Clear ();
				foreach (string k in st) {
					FoundedBeacons.Add (k);
					AllFound.Add (k);
				}
			}

		}

		public void ClearCache ()
		{
			CrossSettings.Current.AddOrUpdateValue<DateTime> ("FoundBeaconRegion", new DateTime (2000, 1, 1));
			CrossSettings.Current.AddOrUpdateValue<DateTime> ("FoundedBeacons", new DateTime (2000, 1, 1));
			DoInit ();
		}

		public void SaveFoundedBeacons ()
		{
			CrossSettings.Current.AddOrUpdateValue<DateTime> ("FoundedBeacons", DateTime.Now);
			string s = string.Empty;
			foreach (string k in FoundedBeacons) {
				if (s.Length == 0)
					s = k;
				else
					s += "," + k;
			}
			CrossSettings.Current.AddOrUpdateValue<string> ("FoundedBeaconsList", s);

		}

		int rssi;
		List<string> founded = new List<string> ();

		public void FoundBeacons (List<OneBeacon> beacons)
		{
			if (!foundBeaconRegion) {
				foundBeaconRegion = true;
				CrossSettings.Current.AddOrUpdateValue<DateTime> ("FoundBeaconRegion", DateTime.Now);
				Global.MainAppPage.DisplayAlert ("Droid Builders .Fr", "Il y a des Droid Builders dans les environs !", "Ok");
			}
			lock (BeaconsLock) {
				founded.Clear ();
				CurrentBeacons.Clear ();
				foreach (OneBeacon b in beacons) {
					rssi = int.Parse (b.Rssi);
					if (rssi == 0) continue;
					major = ToHex4 (b.Major);
					minor = ToHex4 (b.Minor);
					id = major + "." + minor;
					b.Major = major;
					b.Minor = minor;

					if (rssi > -FOUNDRSSI && !FoundedBeacons.Contains (id)) {
						FoundedBeacons.Add (id);
						founded.Add (id);
						AllFound.Add (id);
					}
					/*
					if (ViewedBeacons.ContainsKey (id))
						ViewedBeacons [id] = b;
					else ViewedBeacons.Add (id, b); */
					if (CurrentBeacons.ContainsKey (id))
						CurrentBeacons [id] = b;
					else CurrentBeacons.Add (id, b);
					//Tools.Trace ($"******************************** Beacons: {id} {b.Rssi}");

					//OneBeacon f = null;
					bool found = false;
					foreach (OneBeacon o in AllVisible) {
						if (o.Major == major && o.Minor == minor) {
							o.Rssi = b.Rssi;
							int p = b.Description.ToLower ().IndexOf ("proxim");
							if (p > 0)
								o.Description = b.Description.Substring (p);
							else
								o.Description = b.Description;
							found = true;
							break;
						}
					}
					if (!found)
						AllVisible.Add (b);
					//if (f != null) AllVisible.Remove (f);
				}

				if (founded.Count > 0) {
					SaveFoundedBeacons ();
					if (founded.Count == 1) {
						Global.MainAppPage.DisplayAlert ("Bravo !", $"Vous avez découvert {founded [0]}", "Ok");
					} else {
						string s = string.Empty;
						foreach (string k in founded) {
							if (s.Length == 0)
								s = k;
							else
								s += ", " + k;
						}
						Global.MainAppPage.DisplayAlert ("Félicitations !", $"Vous avez découvert {s}", "Ok");
					}

				}

			}
		}

	}
}