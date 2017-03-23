using System;
using System.Collections.Generic;
using System.IO;

namespace CreateiBeacon
{
	public class Settings
	{
		const string FILENAME = "settings.txt";

		private Dictionary<string, string> settings = null;

		public Settings ()
		{
			settings = new Dictionary<string, string> ();
			DoLoadSettings ();
		}

		private void DoLoadSettings ()
		{
			settings.Clear ();
			if (!File.Exists (FILENAME)) return;
			string [] all = File.ReadAllLines (FILENAME);
			foreach (string s in all) {
				int pos = s.IndexOf ("=");
				if (pos > 0) {
					string key = s.Substring (0, pos).Trim ();
					string value = s.Substring (pos + 1).Trim ();
					settings.Add (key, value);
				}
			}
		}

		private void DoSaveSettings ()
		{
			string [] content = new string [settings.Count];
			int index = 0;
			foreach (KeyValuePair<string, string> kvp in settings) {
				content [index++] = $"{kvp.Key}={kvp.Value}";
			}
			File.WriteAllLines (FILENAME, content);
		}

		public string ValueOf (string key, string defaultValue)
		{
			if (!settings.ContainsKey (key)) return defaultValue;
			return settings [key];
		}

		public string ValueOf (string key)
		{
			return ValueOf (key, string.Empty);
		}

		public int ValueIntOf (string key)
		{
			string v = ValueOf (key, "0");
			return Convert.ToInt16 (v);
		}

		public void SetValue (string key, string value)
		{
			if (settings.ContainsKey (key))
				settings [key] = value;
			else
				settings.Add (key, value);
			DoSaveSettings ();
		}

		public void SetValueInt (string key, int value)
		{
			SetValue (key, value.ToString ());
		}

	}
}