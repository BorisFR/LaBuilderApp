using System;
using System.Collections.Generic;
using Android.Runtime;
using EstimoteSdk;
using LaBuilderApp.Droid;
using Xamarin.Forms;
using Android.App;

[assembly: Dependency (typeof (Beacons))]

namespace LaBuilderApp.Droid
{
	public class Beacons : IBeacons
	{

		object objectLock = new Object ();
		event FoundBeacons theEvent;
		FoundBeacons eh;
		event BeaconInfo theInfo;
		BeaconInfo ti;


		event FoundBeacons IBeacons.FoundBeacons {
			add {
				lock (objectLock) {
					theEvent += value;
				}
			}
			remove {
				lock (objectLock) {
					theEvent -= value;
				}
			}
		}

		event BeaconInfo IBeacons.BeaconInfo {
			add {
				lock (objectLock) {
					theInfo += value;
				}
			}
			remove {
				lock (objectLock) {
					theInfo += value;
				}
			}
		}

		public void SendMsg (string text)
		{
			if (ti != null) ti (text);
		}

		public void GotBeacons (List<OneBeacon> thelist)
		{
			if (thelist.Count > 0)
				if (eh != null)
					eh (thelist);
				else {
					if (ti != null) ti ("no beacon");
				}
		}

		void IBeacons.Init (string uuid, string regionName)
		{
			eh = theEvent;
			ti = theInfo;
			MainActivity.TheBeacons = this;
		}

		void IBeacons.Start ()
		{
		}

		void IBeacons.Stop ()
		{
		}

	}
}