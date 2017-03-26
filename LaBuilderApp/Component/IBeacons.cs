using System;
using System.Collections.Generic;

namespace LaBuilderApp
{
	public class OneBeacon
	{
		public string Major;
		public string Minor;
		public string Rssi;
		public string Description;
	}

	public delegate void FoundBeacons (List<OneBeacon> beacons);
	public delegate void BeaconInfo (string text);

	public interface IBeacons
	{
		event FoundBeacons FoundBeacons;
		event BeaconInfo BeaconInfo;
		void Init (string uuid, string regionName);
		void Start ();
		void Stop ();

	}
}