using System;
using CoreLocation;
using Estimote;
using Foundation;
using LaBuilderApp.iOS;
using UIKit;
using Xamarin.Forms;
using System.Collections.Generic;

[assembly: Dependency (typeof (Beacons))]

namespace LaBuilderApp.iOS
{
	public class Beacons : IBeacons
	{

		BeaconManager beaconManager;
		CLBeaconRegion region;

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

		void IBeacons.Init (string uuid, string regionName)
		{
			eh = theEvent;
			ti = theInfo;
			try {
				beaconManager = new BeaconManager ();
				beaconManager.ReturnAllRangedBeaconsAtOnce = true;
				var theUUID = new NSUuid (uuid); //"E5CAF8CF-590C-42DC-9CF0-2929552156A7"); //8492E75F-4FD6-469D-B132-043FE94921D8");
				region = new CLBeaconRegion (theUUID, regionName);
			} catch (Exception err) {

			}
		}

		void IBeacons.Start ()
		{
			try {
				beaconManager.EnteredRegion += (sender, e) => {
					beaconManager.StartRangingBeaconsInRegion (region);
				};
				beaconManager.ExitedRegion += (sender, e) => {
					beaconManager.StopRangingBeaconsInRegion (region);
				};
				beaconManager.RangedBeacons += (sender2, e2) => {
					//new UIAlertView ("Beacons Found", "Just found: " + e.Beacons.Length + " beacons.", null, "OK").Show ();
					List<OneBeacon> thelist = new List<OneBeacon> ();
					foreach (CLBeacon b in e2.Beacons) {
						string major = b.Major.ToString ();
						string minor = b.Minor.ToString ();
						string rssi = b.Rssi.ToString ();
						thelist.Add (new OneBeacon () { Major = major, Minor = minor, Rssi = rssi, Description = b.Description });
					}
					if (thelist.Count > 0)
						if (eh != null)
							eh (thelist);
						else {
							if (ti != null) ti ("no beacon");
						}
				};
				beaconManager.AuthorizationStatusChanged += (sender, e) => {
					if (ti != null) ti ("AuthorizationStatusChanged");
					StartRangingBeacons ();
				};
				beaconManager.StartMonitoringForRegion (region);
				beaconManager.StartRangingBeaconsInRegion (region);
			} catch (Exception err) {

			}
		}

		void IBeacons.Stop ()
		{
			beaconManager.StopRangingBeaconsInRegion (region); // .StopRangingBeaconsInAllRegions ();
			beaconManager.StopMonitoringForRegion (region); // .StopMonitoringForAllRegions ();
		}

		private void StartRangingBeacons ()
		{
			var status = BeaconManager.AuthorizationStatus;
			if (status == CLAuthorizationStatus.NotDetermined) {
				if (ti != null) ti ("NotDetermined");
				if (!UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
					beaconManager.StartRangingBeaconsInRegion (region);
				} else {
					beaconManager.RequestAlwaysAuthorization ();
				}
			} else if (status == CLAuthorizationStatus.Authorized) {
				if (ti != null) ti ("Authorized");
				beaconManager.StartRangingBeaconsInRegion (region);
			} else if (status == CLAuthorizationStatus.Denied) {
				if (ti != null) ti ("You have denied access to location services. Change this in app settings.");
				//new UIAlertView ("Access Denied", "You have denied access to location services. Change this in app settings.", null, "OK").Show ();
			} else if (status == CLAuthorizationStatus.Restricted) {
				if (ti != null) ti ("You have no access to location services.");
				//new UIAlertView ("Location Not Available", "You have no access to location services.", null, "OK").Show ();
			}
		}

	}
}