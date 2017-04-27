using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LaBuilderApp
{
	public class OneBeacon : INotifyPropertyChanged
	{
		public string Major;
		public string Minor;
		private string rssi; public string Rssi { get { return rssi; } set { rssi = value; RaisePropertyChanged (); RaisePropertyChanged ("Info"); } }
		private string description; public string Description { get { return description; } set { description = value; RaisePropertyChanged (); } }

		public string Info { get { return $"{Major}.{Minor} - {Rssi}"; } }

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged ([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (caller));

			}
		}
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